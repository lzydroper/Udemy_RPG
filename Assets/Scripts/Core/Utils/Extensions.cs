using UnityEngine;

namespace Core.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// 检查游戏物体是否在指定的 LayerMask 中
        /// </summary>
        public static bool IsInLayerMask(this Component comp, LayerMask mask)
        {
            return (mask.value & (1 << comp.gameObject.layer)) != 0;
        }
    }
}