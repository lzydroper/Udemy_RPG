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
        private UITreeNodeConHandler uiTreeConHandler;
        
        [Header("Lock Details")]
        [SerializeField] public UITreeNode[] neededNodes;
        [SerializeField] public UITreeNode[] conflictNodes;
        [Header("Skill Details")]
        [SerializeField] public SkillDataSo skillDataSo;
        [SerializeField] private string skillName;
        [SerializeField] private int skillCost;
        [SerializeField] private Image skillIcon;
        [Header("Color Details")]
        [SerializeField] private Color hoverColor = new(0.9f, 0.9f, 0.9f, 1f);
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
            uiTreeConHandler = GetComponent<UITreeNodeConHandler>();
            rct = GetComponent<RectTransform>();
            // Test
            skillIcon.color = lockedColor;
        }

        private void OnValidate()
        {
            if (skillDataSo == null) return;
            skillName = skillDataSo.Name;
            skillIcon.sprite = skillDataSo.Icon;
            skillCost = skillDataSo.Cost;
            gameObject.name = $"UITreeNode - {skillName}";
        }

        private void OnDisable()
        {
            if (!isDoneUp)
                SetIconColor(lockedColor);
        }

        #endregion
        
        #region PrivateMethods

        private bool CanUnlock()
        {
            if (isLocked || isDoneUp) return false;
            
            if (!uiCanvas.uiTreeSkill.EnoughSkillPoints(skillCost)) return false;

            foreach (var node in neededNodes)
            {
                if (!node.isDoneUp) return false;
            }

            foreach (var node in conflictNodes)
            {
                if (node.isDoneUp) return false;
            }
            
            return true;
        }
        
        private void SetIconColor(Color color) => skillIcon.color = color;

        private void LockConflictNodes()
        {
            foreach (var node in conflictNodes)
            {
                node.isLocked = true;
            }
        }

        private void DoneUp()
        {
            isDoneUp = true;
            SetIconColor(doneUpColor);
            uiCanvas.uiTreeSkill.RemoveSkillPoints(skillCost);
            LockConflictNodes();
            uiTreeConHandler.UpdateConColor(true);
        }
        
        #endregion
        
        #region PointerMethods

        public void OnPointerEnter(PointerEventData eventData)
        {
            uiCanvas.uiTooltipSkill.Show(true, rct, this);
            if (!CanUnlock()) return;
            SetIconColor(hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            uiCanvas.uiTooltipSkill.Show(false, rct);
            if (!CanUnlock()) return;
            SetIconColor(lockedColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (CanUnlock())
                DoneUp();
            else if (isLocked)
                uiCanvas.uiTooltipSkill.AlertEffect();
        }

        #endregion
        
        // TODO: isLocked需要归他管吗？
        public void Refund()
        {
            isDoneUp = false;
            SetIconColor(lockedColor);
            uiCanvas.uiTreeSkill.IncreaseSkillPoints(skillCost);
            uiTreeConHandler.UpdateConColor(false);
        }
    }
}