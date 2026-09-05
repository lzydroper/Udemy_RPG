using System;
using System.Collections.Generic;
using System.Threading;
using Core.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Core.Pool
{
    public class GameObjectPool
    {
        private CancellationTokenSource cts;
        private readonly Dictionary<int, CancellationTokenSource> _objCtsDict = new();
        private readonly Dictionary<int, IPoolRent> _rentDict = new();
        private readonly Dictionary<int, IPoolReturn> _returnDict = new();
        // 防止重复Return
        private readonly HashSet<int> _poolSet = new();
        public bool IsDestroyed { get; private set; }
        public string PoolName { get; private set; }
        public Transform RootTransform { get; private set; }
        public Queue<GameObject> PoolQueue { get; } = new();
        // 对应的prefab，初始化后不再变化
        public GameObject Prefab { get; private set; }
        // 池创建出的对象总数，仅在 Expand 增加，Destroy 后失效
        public int Count { get; private set; }
        // 最大上限，初始化后不再变化
        public int Capacity { get; private set; }
        // 单次扩容步数，初始化后不再变化
        public int Step { get; private set; }
        // 生存时间，-1表示无限
        public float Duration { get; private set; }

        public void Initialize(string poolName, Transform upperRootTransform, GameObject prefab,
            int step = 8, int capacity = -1, int prewarm = 0, float duration = -1)
        {
            cts = new();
            Prefab = prefab;
            if (capacity != -1 )
            {
                // 扩张步长最大为容量上限
                if (step > capacity)
                    step = capacity;
                // 预热个数最大为容量上限
                if (prewarm > capacity)
                    prewarm = capacity;
            }
            // 创建存放父级
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(upperRootTransform);
            // 重置
            PoolName = poolName;
            RootTransform = gameObject.transform;
            RootTransform.name = $"[Pool] {PoolName}";
            Step = step;
            Capacity = capacity;
            Duration = duration;
            // 预热
            Expand(prewarm);
        }

        private void Expand(int target)
        {
            // 不能超过最大上限
            if (Capacity != -1 && Capacity <= Count)
                return;
            // 需要增加的个数，最小为0，最大为剩余可增加的个数
            int count = Capacity == -1
                ? Mathf.Max(0, target)
                : Mathf.Clamp(target, 0, Capacity - Count);
            for (int i = 0; i < count; i++)
            {
                GameObject gameObject = Object.Instantiate(Prefab, RootTransform);
                gameObject.name = PoolName;
                if (gameObject.TryGetComponent(out IPoolRent rent))
                    _rentDict[gameObject.GetInstanceID()] = rent;
                if (gameObject.TryGetComponent(out IPoolReturn ret))
                    _returnDict[gameObject.GetInstanceID()] = ret;
                Push(gameObject);
                Count++;
            }
        }

        private void Push(GameObject gameObject)
        {
            PoolQueue.Enqueue(gameObject);
            _poolSet.Add(gameObject.GetInstanceID());
            gameObject.transform.SetParent(RootTransform);
            gameObject.SetActive(false);
        }

        private GameObject Rent(Vector3 position = default, Quaternion rotation = default, 
            PoolParentOverride parent = default)
        {
            if (PoolQueue.Count == 0)
                Expand(Step);
            if (PoolQueue.Count == 0)
                return null;
            var gameObject = PoolQueue.Dequeue();
            _poolSet.Remove(gameObject.GetInstanceID());
            // 未指定 parent 时保留池默认层级；显式传 null 时才脱离父级。
            if (parent.IsSpecified)
            {
                gameObject.transform.SetParent(parent.Value);
                if (parent.Value == null)
                {
                    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
                }
            }
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            if (_rentDict.TryGetValue(gameObject.GetInstanceID(), out IPoolRent rent))
                rent.OnRent();
            // 做完全部后才active
            gameObject.SetActive(true);
            return gameObject;
        }

        public bool TryReturn(GameObject gameObject)
        {
            // 判空，并防止重复入队
            if (gameObject is null || _poolSet.Contains(gameObject.GetInstanceID()))
                return false;
            var id = gameObject.GetInstanceID();
            if (_objCtsDict.TryGetValue(id, out var objCts))
            {
                objCts.Cancel();
                objCts.Dispose();
                _objCtsDict.Remove(id);
            }
            if (_returnDict.TryGetValue(id, out IPoolReturn ret))
                ret.OnReturn();
            Push(gameObject);
            return true;
        }

        public bool TryRent(out GameObject gameObject, Vector3 position = default, Quaternion rotation = default, 
            PoolParentOverride parent = default)
        {
            gameObject = Rent(position, rotation, parent);
            return gameObject is not null;
        }

        public async UniTaskVoid ReturnAsync(GameObject gameObject, float duration = -1)
        {
            // 不接受生存时间不合法选项
            duration = duration < 0 ? Duration : duration;
            if (duration < 0 || gameObject is null) return;
            var id = gameObject.GetInstanceID();
            // 如果上次遗留了任务，先清理
            if (_objCtsDict.TryGetValue(id, out var oldCts))
            {
                oldCts.Cancel();
                oldCts.Dispose();
            }
            // 为本次延迟回池生成专属 CTS
            var objCts = new CancellationTokenSource();
            _objCtsDict[id] = objCts;

            try
            {
                // 联动：整个池销毁 或 该物体被提前回收 时，都会触发取消
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, objCts.Token);

                bool isCanceled = await UniTask
                    .Delay(TimeSpan.FromSeconds(duration), cancellationToken: linkedCts.Token)
                    .SuppressCancellationThrow();

                // 如果被取消（提前被打死回池了）或物体已不存在，直接退出
                if (isCanceled)
                    return;

                TryReturn(gameObject);
            }
            finally
            {
                // 延时结束后，清理字典记录
                if (_objCtsDict.TryGetValue(id, out var current) && current == objCts)
                {
                    _objCtsDict.Remove(id);
                    objCts.Dispose();
                }
            }
        }
        
        public void Destroy()
        {
            IsDestroyed = true;
            cts.Cancel();
            cts.Dispose();
            PoolQueue.Clear();
            _poolSet.Clear();
            if (RootTransform != null)
            {
                Object.Destroy(RootTransform.gameObject);
                RootTransform = null;
            }
            Prefab = null;
            Count = 0;
        }
    }
}