using TMPro;
using UnityEngine;

namespace UI
{
    public class UITooltipSkill : UITooltip
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI skillName;
        [SerializeField] private TextMeshProUGUI skillDescription;
        [SerializeField] private TextMeshProUGUI skillRequirement;

        #endregion

        #region LifecycleMethods

        

        #endregion

        public override void Show(bool enable, RectTransform targetRct)
        {
            base.Show(enable, targetRct);
        }

        public void Show(bool enable, RectTransform targetRct, SkillDataSo so)
        {
            base.Show(enable, targetRct);
            if (enable == false) return;
            skillName.text = so.Name;
            skillDescription.text = so.Description;
            skillRequirement.text = "Requirement:\n"
                                    + " -" + so.Cost + " skill points.";
        }
    }
}