using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UITreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        #region Variables
        
        private UICanvas uiCanvas;
        private RectTransform rct;
        
        [SerializeField] private SkillDataSo skillDataSo;
        [SerializeField] private string skillName;
        [SerializeField] private Image skillIcon;

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
            uiCanvas = GetComponentInParent<UICanvas>();
            rct = GetComponent<RectTransform>();
            // Test
            skillIcon.color = lockedColor;
        }

        private void OnValidate()
        {
            if (skillDataSo == null) return;
            skillName = skillDataSo.Name;
            skillIcon.sprite = skillDataSo.Icon;
            gameObject.name = $"UITreeNode - {skillName}";
        }

        #endregion

        private bool CanInteract()
        {
            if (isLocked || isDoneUp) return false;
            return true;
        }
        
        private void SetIconColor(Color color) => skillIcon.color = color;
        
        #region PointerMethods

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanInteract()) return;
            uiCanvas.uiTooltipSkill.Show(true, rct, skillDataSo);
            SetIconColor(hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanInteract()) return;
            uiCanvas.uiTooltipSkill.Show(false, rct);
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