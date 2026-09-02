using System;
using SkillSystem.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillSystem.UI
{
    public class SkillUINode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        #region Variables
    
        public RectTransform rectTransform;
        public SkillUITree skillUITree;
        [Header("Skill Details")]
        [SerializeField] public SkillDataSo skillData;
        [SerializeField] private Image skillIcon;
        [Header("Color Details")]
        [SerializeField] private Color hoverColor = new(0.9f, 0.9f, 0.9f, 1f);
        [SerializeField] private Color lockedColor = Color.gray;
        [SerializeField] private Color unlockedColor = Color.white;
        
        #endregion

        public void Bind(SkillDataSo skillData, SkillUITree skillUITree)
        {
            this.skillData = skillData;
            this.skillUITree = skillUITree;
        }

        public void Refresh()
        {
            if (skillData == null) return;
            gameObject.name = $"SkillUINode - {skillData.displayName}";
            skillIcon.sprite = skillData.icon;
        }

        private void Start()
        {
            SetIconColor(skillUITree.skillManager.IsSkillUnlocked(skillData) ? unlockedColor : lockedColor);
        }

        private void OnDisable()
        {
            SetIconColor(skillUITree.skillManager.IsSkillUnlocked(skillData) ? unlockedColor : lockedColor);
        }

        // 对外refund时调用
        public void SetIconLocked() => SetIconColor(lockedColor);
    
        public void SetPosition(Vector2 position) => rectTransform.anchoredPosition = position;
    
        private void SetIconColor(Color color) => skillIcon.color = color;

        #region HelpingMethods

        private bool IsSkillAvailable() => skillUITree.skillManager.IsSkillAvailable(skillData);
        private bool IsSkillUnlocked() => skillUITree.skillManager.IsSkillUnlocked(skillData);
        private bool IsSkillConflict() => skillUITree.skillManager.IsSkillConflict(skillData);

        #endregion
    
        #region PointerMethods

        public void OnPointerEnter(PointerEventData eventData)
        {
            skillUITree.SkillUITooltip.Show(true, rectTransform, skillData);
            if (!IsSkillAvailable()) return;
            SetIconColor(hoverColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            skillUITree.SkillUITooltip.Show(false, rectTransform);
            if (!IsSkillAvailable()) return;
            SetIconColor(lockedColor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (skillUITree.TryUnlockSkill(skillData))
            {
                SetIconColor(unlockedColor);
                skillUITree.SkillUITooltip.Show(true, rectTransform, skillData);
            }
            else
                skillUITree.SkillUITooltip.ShowAlertEffect(skillData);
        }

        #endregion
    }
}