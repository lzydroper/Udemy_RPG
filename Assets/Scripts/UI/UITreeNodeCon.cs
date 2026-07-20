using UnityEngine;

namespace UI
{
    public class UITreeNodeCon : MonoBehaviour
    {
        #region Variables

        [SerializeField] private RectTransform rotationPoint;
        [SerializeField] private RectTransform connectionLine;
        [SerializeField] private RectTransform childConPoint;

        #endregion

        public void SetConnectionLine(UITreeNodeConDirType direction, float length)
        {
            if (direction == UITreeNodeConDirType.None) length = 0;
            rotationPoint.localRotation = Quaternion.Euler(0, 0, GetAngleByType(direction));
            connectionLine.sizeDelta = new Vector2(connectionLine.sizeDelta.x, length);
        }

        public Vector2 TransformChildNodePos(RectTransform targetRct)
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
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (
                    targetRct.parent as RectTransform,
                    childConPoint.position,     // Screen Space - Overlay下，RecTransform的position等同于screenPosition
                    null,                     // camera = null 时 Unity 认为你传入的是 Overlay 模式下的屏幕像素坐标
                    out var localPoint
                );
            return localPoint;
        }

        private float GetAngleByType(UITreeNodeConDirType type) => type switch
        {
            UITreeNodeConDirType.Up => 180f,
            UITreeNodeConDirType.UpRight => 135f,
            UITreeNodeConDirType.Right => 90f,
            UITreeNodeConDirType.DownRight => 45f,
            UITreeNodeConDirType.Down => 0f,
            UITreeNodeConDirType.DownLeft => -45f,
            UITreeNodeConDirType.Left => -90f,
            UITreeNodeConDirType.UpLeft => -135f,
            _ => 0f,
        };
    }
}