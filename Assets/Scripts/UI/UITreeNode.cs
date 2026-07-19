using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        #region Variables

        [SerializeField] private Image icon;
        [SerializeField] private Color hoverColor = Color.white * 0.9f;
        // 锁住无法交互
        public bool isLocked;
        [SerializeField] private Color lockedColor = Color.gray;
        // 用完无需交互，作为提醒
        public bool isDoneUp;
        [SerializeField] private Color doneUpColor = Color.white;
        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            // Test
            icon.color = lockedColor;
        }

        #endregion

        private bool CanInteract()
        {
            if (isLocked || isDoneUp) return false;
            return true;
        }
        
        private void SetIconColor(Color color) => icon.color = color;
        
        #region PointerMethods

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanInteract()) return;
            SetIconColor(hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanInteract()) return;
            SetIconColor(lockedColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!CanInteract()) return;
            isDoneUp = true;
            SetIconColor(doneUpColor);
        }
        
        #endregion
    }
}