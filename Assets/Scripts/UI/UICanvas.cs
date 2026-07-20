using System;
using UnityEngine;

namespace UI
{
    public class UICanvas : MonoBehaviour
    {
        #region Variables

        public UITooltipSkill uiTooltipSkill;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            uiTooltipSkill = GetComponentInChildren<UITooltipSkill>();
        }

        #endregion
    }
}