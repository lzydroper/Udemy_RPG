using System.Collections;
using System.Text;
using Core.Utils;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UITooltipSkill : UITooltip
    {
        #region Variables

        private UICanvas uiCanvas;
        
        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI skillDescription;
        [SerializeField] private TextMeshProUGUI skillRequirement;
        [Space]
        [SerializeField] private Color normColor;
        [SerializeField] private Color alrtColor;
        [SerializeField] private Color imptColor;
        private string normHex;
        private string alrtHex;
        private string imptHex;
        [SerializeField] private string lockedHint = "You have chosen another path, the skill is locked.";
        
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

        public void AlertEffect()
            => this.RestartCoroutine(ref alertEffectCoroutine, 
                AlertEffectCo(skillRequirement, 0.3f, 3));
        
        private IEnumerator AlertEffectCo(TextMeshProUGUI text, float interval, int count)
        {
            var alrt = GetColorString(alrtHex, lockedHint);
            var impt = GetColorString(imptHex, lockedHint);
            for (int i = 0; i < count; i++)
            {
                text.text = alrt;
                yield return new WaitForSeconds(interval);

                text.text = impt;
                yield return new WaitForSeconds(interval);
            }
        }

        public void Show(bool enable, RectTransform targetRct, UITreeNode node)
        {
            base.Show(enable, targetRct);
            if (enable == false) return;
            skillName.text = node.skillDataSo.Name;
            skillDescription.text = node.skillDataSo.Description;
            skillRequirement.text = node.isLocked
                ? GetColorString(imptHex, lockedHint)
                : BuildRequirement(node.skillDataSo.Cost, node.neededNodes, node.conflictNodes);
        }

        private string BuildRequirement(int cost, UITreeNode[] neededNodes, UITreeNode[] conflictNodes)
        {
            var sb = new StringBuilder();
            var costHex = uiCanvas.uiTreeSkill.EnoughSkillPoints(cost) ? normHex : alrtHex;
            sb.AppendLine("Requirement:");
            sb.AppendLine(GetColorString(costHex, cost.ToString(), "- ", " skill points."));

            foreach (var node in neededNodes)
            {
                var nodeHex = node.isDoneUp ? normHex : alrtHex;
                sb.AppendLine(GetColorString(nodeHex, node.skillDataSo.Name, "- "));
            }

            if (neededNodes.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine(GetColorString(imptHex, "Locks out:"));
                foreach (var node in conflictNodes)
                {
                    sb.AppendLine(GetColorString(imptHex, node.skillDataSo.Name, "- "));
                }
            }
            
            return sb.ToString();
        }

        private string GetColorString(string colorHex, string content, string prefix = "", string suffix = "") 
            => $"<color=#{colorHex}>{prefix}{content}{suffix}</color>";
    }
}