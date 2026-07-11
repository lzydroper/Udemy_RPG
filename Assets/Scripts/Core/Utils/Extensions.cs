using System.Collections;
using UnityEngine;

namespace Core.Utils
{
    public static class Extensions
    {
        #region PoolManager
        
        public static void Return(this GameObject obj, string poolName = null)
        {
            GameManager.Pool.Return(obj, poolName);
        }
        
        public static void Return(this Component component, string poolName = null)
        {
            GameManager.Pool.Return(component.gameObject, poolName);
        }

        public static void ReturnAsync(this GameObject obj, string poolName = null, float duration = -1)
        {
            GameManager.Pool.ReturnAsync(obj, poolName, duration);
        }

        public static void ReturnAsync(this Component component, string poolName = null, float duration = -1)
        {
            GameManager.Pool.ReturnAsync(component.gameObject, poolName, duration);
        }
        
        #endregion
        
        /// <summary>
        /// 检查游戏物体是否在指定的 LayerMask 中
        /// </summary>
        public static bool IsInLayerMask(this Component comp, LayerMask mask)
        {
            return (mask.value & (1 << comp.gameObject.layer)) != 0;
        }

        public static void RestartCoroutine(this MonoBehaviour mono, ref Coroutine coroutine, IEnumerator routine)
        {
            if (coroutine != null)
                mono.StopCoroutine(coroutine);
            coroutine = mono.StartCoroutine(routine);
        }
    }
}