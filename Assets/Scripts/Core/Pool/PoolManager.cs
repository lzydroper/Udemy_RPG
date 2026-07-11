using System.Collections.Generic;
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
            int step = 4, int capacity = -1, int prewarm = 0, float duration = -1)
        {
            if (PoolDict.ContainsKey(poolName))
                return;
            var pool = new GameObjectPool();
            pool.Initialize(poolName, _poolRootTransform, prefab, step, capacity, prewarm, duration);
            PoolDict.Add(poolName, pool);
        }

        public void Register(PoolDataSO poolData) 
            => Register(poolData.poolName, poolData.prefab, poolData.step, poolData.capacity, poolData.prewarm, poolData.duration);

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
            Vector3 position = default, Quaternion rotation = default, PoolParentOverride parent = default)
        {
            gameObject = null;
            if (PoolDict.TryGetValue(poolName, out pool) && pool.TryRent(out gameObject, position, rotation, parent))
                return true;
            return false;
        }

        public bool TryRent(string poolName, out GameObject gameObject,
            Vector3 position = default, Quaternion rotation = default, PoolParentOverride parent = default)
            => TryRent(poolName, out _, out gameObject, position, rotation, parent);

        public bool TryRent<T>(string poolName, out Component component,
            Vector3 position = default, Quaternion rotation = default, PoolParentOverride parent = default)
            where T : Component
        {
            component = null;
            if (TryRent(poolName, out _, out var gameObject, position, rotation, parent) &&
                gameObject.TryGetComponent(out component))
                return true;
            return false;
        }

        public void RentAsync(string poolName, float duration = -1,
            Vector3 position = default, Quaternion rotation = default, PoolParentOverride parent = default)
        {
            if (TryRent(poolName, out var pool, out var gameObject, position, rotation, parent))
            {
                pool.ReturnAsync(gameObject, duration).Forget();
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
                pool.TryReturn(gameObject);
            }
        }

        public void ReturnAsync(GameObject gameObject, string poolName = null, float duration = -1)
        {
            if (gameObject == null)
                return;
            poolName ??= gameObject.name;
            if (PoolDict.TryGetValue(poolName, out var pool))
            {
                pool.ReturnAsync(gameObject, duration).Forget();
            }
        }
        
        #endregion
    }
}