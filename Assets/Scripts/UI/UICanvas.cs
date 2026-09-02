using SkillSystem.UI;
using UnityEngine;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        #region Variables

        public SkillUITree skillUITree;
        public SkillUITooltip skillUITooltip;
        public bool ifShowSkillUITree;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            skillUITooltip = GetComponentInChildren<SkillUITooltip>(true);
            skillUITree = GetComponentInChildren<SkillUITree>(true);
        }

        #endregion

        public void ToggleUITreeSkill()
        {
            ifShowSkillUITree = !ifShowSkillUITree;
            skillUITree.gameObject.SetActive(ifShowSkillUITree);
            skillUITooltip.Show(false, null);
        }
    }
}