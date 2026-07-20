using UnityEngine;

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
    
    [System.Serializable]
    public class UITreeNodeConDetail
    {
        public UITreeNodeConDirType direction = UITreeNodeConDirType.Down;
        [Range(100f, 300f)] public float length = 100f;
        public UITreeNodeConHandler childNode;
    }
    
    public class UITreeNodeConHandler : MonoBehaviour
    {
        #region Variables

        private RectTransform rct;

        [SerializeField] private UITreeNodeConDetail[] details;
        [SerializeField] private UITreeNodeCon[] connections;

        #endregion

        #region LifecycleMethods

        private void OnValidate()
        {
            if (rct == null) rct = GetComponent<RectTransform>();
            if (details.Length != connections.Length) return;
            UpdateConnections();
        }

        #endregion
        
        public void SetPosition(Vector2 position) => rct.anchoredPosition = position;

        private void UpdateConnections()
        {
            for (int i = 0; i < details.Length; i++)
            {
                var detail = details[i];
                var connection = connections[i];
                connection.SetConnectionLine(detail.direction, detail.length);
                detail.childNode.SetPosition(connection.TransformChildNodePos(rct));
            }
        }
    }
}