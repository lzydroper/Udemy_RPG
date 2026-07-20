using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum UITreeNodeConDirType
    {
        None,
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }
    
    [Serializable]
    public class UITreeNodeConDetail
    {
        public UITreeNodeConDirType direction = UITreeNodeConDirType.Down;
        [Range(100f, 300f)] public float length = 100f;
        public UITreeNodeConHandler childNode;
        [Range(-45f, 45f)] public float rotationOffset;
    }
    
    public class UITreeNodeConHandler : MonoBehaviour
    {
        #region Variables

        private RectTransform rct;

        [SerializeField] private UITreeNodeConDetail[] details;
        [SerializeField] private UITreeNodeCon[] connections;

        /// <summary>
        /// 连接至这个节点的线
        /// </summary>
        private Image connectLine;
        private Color connectLineColor;
        
        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            if (connectLine != null) connectLineColor = connectLine.color;
        }

        private void OnValidate()
        {
            if (rct == null) rct = GetComponent<RectTransform>();
            if (details.Length <= 0) return;
            if (details.Length != connections.Length) return;
            UpdateConnections();
        }

        #endregion
        
        public void SetPosition(Vector2 position) => rct.anchoredPosition = position;
        public void SetConImage(Image image) => connectLine = image;

        // DONE: 毫无道理，这里为什么要enable
        // 因为非常久远以后的功能需要，妈的一开始不说清楚，煞笔教程
        public void UpdateConColor(bool enable)
        {
            if (connectLine == null) return;
            connectLine.color = enable ? Color.white : connectLineColor;
        }

        private void UpdateConnections()
        {
            for (int i = 0; i < details.Length; i++)
            {
                var detail = details[i];
                var connection = connections[i];
                var conImage = connection.GetConnectionLineImage();
                connection.SetConnectionLine(detail.direction, detail.length, detail.rotationOffset);
                detail.childNode?.SetPosition(connection.TransformChildNodePos(rct));
                detail.childNode?.SetConImage(conImage);
                detail.childNode?.transform.SetAsLastSibling();
            }
        }

        public void UpdateAllCon()
        {
            UpdateConnections();

            foreach (var detail in details)
            {
                if (detail.childNode == null) continue;
                detail.childNode.UpdateConnections();
            }
        }
    }
}