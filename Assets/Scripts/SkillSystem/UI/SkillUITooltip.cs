using System.Collections;
using System.Text;
using Core.Utils;
using SkillSystem.Data;
using TMPro;
using UI;
using UnityEngine;

namespace SkillSystem.UI
{
    public class SkillUITooltip : UITooltip
    {
        #region Variables

        private UICanvas uiCanvas;
        
        [SerializeField] private TextMeshProUGUI skillDisplayName;
        [SerializeField] private TextMeshProUGUI skillDescription;
        [SerializeField] private TextMeshProUGUI skillRequirement;
        [Space]
        [SerializeField] private Color normColor;
        [SerializeField] private Color alrtColor;
        [SerializeField] private Color imptColor;
        private string normHex;
        private string alrtHex;
        private string imptHex;
        [SerializeField] private string conflictHint = "You have chosen another path, the skill is locked.";
        
        private Coroutine alertEffectCoroutine;

        #endregion

        #region LifecycleMethods

        protected override void Awake()
        {
            base.Awake();
            uiCanvas = GetComponentInParent<UICanvas>();
            normHex = ColorUtility.ToHtmlStringRGB(normColor);
            alrtHex = ColorUtility.ToHtmlStringRGB(alrtColor);
            imptHex = ColorUtility.ToHtmlStringRGB(imptColor);
        }

        #endregion

        public void ShowAlertEffect(SkillDataSo skillData)
            => this.RestartCoroutine(ref alertEffectCoroutine, 
                AlertEffectCo(skillData, 0.3f, 3));
        
        private IEnumerator AlertEffectCo(SkillDataSo skillData, float interval, int count)
        {
            var text = BuildRequirement(skillData, true);
            var alrt = GetFormatString(alrtHex, text);
            var impt = GetFormatString(imptHex, text);
            for (int i = 0; i < count; i++)
            {
                skillRequirement.text = alrt;
                yield return new WaitForSeconds(interval);

                skillRequirement.text = impt;
                yield return new WaitForSeconds(interval);
            }
            skillRequirement.text = BuildRequirement(skillData);
        }

        public void Show(bool enable, RectTransform targetRct, SkillDataSo skillData)
        {
            base.Show(enable, targetRct);
            if (enable == false) return;
            skillDisplayName.text = skillData.displayName;
            skillDescription.text = skillData.description;
            skillRequirement.text = BuildRequirement(skillData);
        }

        private string BuildRequirement(SkillDataSo skillData, bool isRaw = false)
        {
            // 若已解锁则无需求
            if (uiCanvas.skillUITree.skillManager.IsSkillUnlocked(skillData))
                return "";
            // 若冲突则冲突提示
            if (uiCanvas.skillUITree.skillManager.IsSkillConflict(skillData))
                return GetFormatString(imptHex, conflictHint, isRaw : isRaw);
            // 正常构造需求内容
            var cost = skillData.cost;
            var prerequisites = skillData.prerequisites;
            var conflicts = skillData.conflicts;
            var sb = new StringBuilder();
            var costHex = uiCanvas.skillUITree.skillManager.EnoughSkillPoints(cost) ? normHex : alrtHex;
            sb.AppendLine("Requirement:");
            sb.AppendLine(GetFormatString(costHex, cost.ToString(), "- ", " skill points.", isRaw));

            foreach (var node in prerequisites)
            {
                var isUnlocked = uiCanvas.skillUITree.skillManager.IsSkillUnlocked(node);
                var nodeHex = isUnlocked ? normHex : alrtHex;
                sb.AppendLine(GetFormatString(nodeHex, node.displayName, "- ", isRaw : isRaw));
            }

            if (conflicts.Length > 0)
            {
                sb.AppendLine();
                var lockHint = GetFormatString(imptHex, "Locks out:", isRaw : isRaw);
                sb.AppendLine(lockHint);
                foreach (var node in conflicts)
                {
                    sb.AppendLine(GetFormatString(imptHex, node.displayName, "- ", isRaw : isRaw));
                }
            }
            
            return sb.ToString();
        }

        private string GetFormatString(string colorHex, string content, string prefix = "", string suffix = "",
            bool isRaw = false)
        {
            return isRaw
                ? $"{prefix}{content}{suffix}"
                : $"<color=#{colorHex}>{prefix}{content}{suffix}</color>";
        }
    }
}