using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Pool
{
    public class PoolManager
    {
        private readonly Transform _poolRootTransform;
        public Dictionary<string, GameObjectPool> PoolDict { get; } = new();

        public PoolManager()
        {
            GameObject root = new GameObject("===Pools===");
            _poolRootTransform = root.transform;
        }

        #region Register
        
        public void Register(string poolName, GameObject prefab,
            int step = 4, int capacity = -1, int prewarm = 0)
        {
            if (PoolDict.ContainsKey(poolName))
                return;
            var pool = new GameObjectPool();
            pool.Initialize(poolName, _poolRootTransform, prefab, step, capacity, prewarm);
            PoolDict.Add(poolName, pool);
        }

        public void Register(PoolDataSO poolData) 
            => Register(poolData.poolName, poolData.prefab, poolData.step, poolData.capacity, poolData.prewarm);
        
        #endregion

        #region Unregister

        public void Unregister(string poolName)
        {
            if (PoolDict.TryGetValue(poolName, out var pool))
            {
                pool.Destroy();
                PoolDict.Remove(poolName);
            }
        }

        public void Unregister()
        {
            foreach (var pool in PoolDict.Values)
            {
                pool.Destroy();
            }
            PoolDict.Clear();
        }

        #endregion

        #region Rent

        private bool TryRent(string poolName, out GameObjectPool pool, out GameObject gameObject,
            Vector3? position = null, Quaternion? rotation = null, PoolParentOverride parent = default)
        {
            gameObject = null;
            if (!PoolDict.TryGetValue(poolName, out pool) || !pool.TryRent(out gameObject, parent))
                return false;
            if (position.HasValue) gameObject.transform.position = position.Value;
            if (rotation.HasValue) gameObject.transform.rotation = rotation.Value;
            return true;
        }

        public GameObject Rent(string poolName,
            Vector3? position = null, Quaternion? rotation = null, PoolParentOverride parent = default)
            => TryRent(poolName, out _, out var gameObject, position, rotation, parent) ? gameObject : null;

        public bool TryRent(string poolName, out GameObject gameObject,
            Vector3? position = null, Quaternion? rotation = null, PoolParentOverride parent = default)
            => TryRent(poolName, out _, out gameObject, position, rotation, parent);

        public async UniTaskVoid Rent(string poolName, float duration,
            Vector3? position = null, Quaternion? rotation = null, PoolParentOverride parent = default)
        {
            if (TryRent(poolName, out var pool, out var gameObject, position, rotation, parent))
            {
                await UniTask.Delay(TimeSpan.FromSeconds(duration));
                // 等待的这会，可能池/物体被销毁了，物体不会无端销毁，关心池因为场景切换注销的问题
                if (!pool.IsDestroyed)
                {
                    pool.Return(gameObject);
                }
            }
        }
        
        #endregion

        #region Return

        public void Return(GameObject gameObject, string poolName = null)
        {
            if (gameObject == null)
                return;
            poolName ??= gameObject.name;
            if (PoolDict.TryGetValue(poolName, out var pool))
            {
                pool.Return(gameObject);
            }
        }

        public async UniTaskVoid Return(GameObject gameObject, float duration, string poolName = null)
        {
            if (gameObject == null)
                return;
            poolName ??= gameObject.name;
            if (PoolDict.TryGetValue(poolName, out var pool))
            {
                await UniTask.WaitForSeconds(duration);
                // 等待的这会，可能池/物体被销毁了，物体不会无端销毁，关心池因为场景切换注销的问题
                if (pool is not { IsDestroyed: true })
                {
                    pool.Return(gameObject);
                }
            }
        }
        
        #endregion
    }
}