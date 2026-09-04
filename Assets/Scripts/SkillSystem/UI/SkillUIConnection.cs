using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SkillSystem.UI
{
    public class SkillUIConnection : MonoBehaviour
    {
        #region Variables

        [Header("Connection")]
        [SerializeField] private Image connectionImage;
        [SerializeField] private RectTransform connectionRoot;
        [SerializeField] private RectTransform connectionRect;

        [Header("Color")]
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color unlockedColor = Color.white;


        [SerializeField] private SkillUINode parentNode;
        [SerializeField] private SkillUINode childNode;

        #endregion

        #region Public Methods

        /// <summary>
        /// 编辑器构建完成后绑定这条连接对应的两个节点。
        /// </summary>
        public void Bind(SkillUINode parentNode, SkillUINode childNode, RectTransform connectionRoot)
        {
            this.parentNode = parentNode;
            this.childNode = childNode;
            this.connectionRoot = connectionRoot;

            RefreshPosition();
        }

        /// <summary>
        /// 根据两个节点的 ConnectionPoint 更新连线位置。
        /// </summary>
        public void RefreshPosition()
        {
            if (parentNode == null || childNode == null || connectionRoot == null)
                return;

            Vector2 sta = GetConnectionPoint(parentNode.rectTransform);
            Vector2 end = GetConnectionPoint(childNode.rectTransform);

            Vector2 direction = end - sta;

            connectionRect.anchoredPosition = (sta + end) * 0.5f;
            connectionRect.sizeDelta = new Vector2(direction.magnitude, connectionRect.sizeDelta.y);
            connectionRect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            
            EditorUtility.SetDirty(this);
        }
    
        /// <summary>
        /// 根据技能状态刷新连线颜色。
        /// </summary>
        public void RefreshState(bool unlocked)
        {
            connectionImage.color = unlocked
                ? unlockedColor
                : lockedColor;
        }

        #endregion

        #region Private Methods

        private Vector2 GetConnectionPoint(RectTransform point)
        {
            // 把世界坐标转成本地坐标，发生了跨越父级坐标系的移动
            // 因为在Transform或RectTransform中，没有真正存储的世界坐标position，其实际为计算、转换而来的
            // 故而inspector面板中显示的是本地坐标，
            // Screen Space - Camera 或 World Space 模式下，"世界坐标"本身还要经过摄像机的透视投影/正交投影换算
            // UI 的布局系统（Layout Group、锚点重算）都是基于 anchoredPosition / sizeDelta 在特定时机
            // （LateUpdate 或者 Canvas Rebuild）重新计算的
            // rebuild 原因：比如父级是个 Layout Group，或者分辨率变了、Canvas Scaler 重算
            // 由于转换层的存在，如果直接修改世界坐标，可能导致：
            // 1. 参考系不同，转换出来的锚点坐标不同，视觉上不正确
            // 2. 某一帧写了世界坐标，unity转换出一个锚点坐标并保存，下一帧如果父级发生rebuild，会使用该锚点坐标重新计算世界坐标，可能与原本
            //    期望的世界坐标不同
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                connectionRoot,
                point.position,    
                null,                           // Screen Space - Overlay下，RecTransform的position等同于screenPosition
                out Vector2 localPosition   // camera = null 时 Unity 认为你传入的是 Overlay 模式下的屏幕像素坐标
            );

            return localPosition;
        }

        #endregion
    }
}