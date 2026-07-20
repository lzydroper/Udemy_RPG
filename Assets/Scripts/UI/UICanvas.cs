using System;
using UnityEngine;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        #region Variables

        [HideInInspector] public UITooltipSkill uiTooltipSkill;
        [HideInInspector] public UITreeSkill uiTreeSkill;
        public bool ifShowUITreeSkill;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            uiTooltipSkill = GetComponentInChildren<UITooltipSkill>();
            uiTreeSkill = GetComponentInChildren<UITreeSkill>(true);
        }

        #endregion

        public void ToggleUITreeSkill()
        {
            ifShowUITreeSkill = !ifShowUITreeSkill;
            uiTreeSkill.gameObject.SetActive(ifShowUITreeSkill);
            uiTooltipSkill.Show(false, null);
        }
    }
}