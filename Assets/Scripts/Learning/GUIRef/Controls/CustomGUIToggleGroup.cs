using UnityEngine;

namespace Learning.GUIRef.Controls
{
    public class CustomGUIToggleGroup : MonoBehaviour
    {
        #region Variables

        [SerializeField] private CustomGUIToggle[] m_toggles;
        private CustomGUIToggle m_selectedToggle;

        #endregion

        #region LifecycleMethods

        private void Start()
        {
            if (m_toggles == null || m_toggles.Length == 0)
                return;
            for (int i = 0; i < m_toggles.Length; i++)
            {
                var toggle = m_toggles[i];
                toggle.onValueChanged.AddListener(value => OnToggleChanged(toggle, value));
            }
        }

        #endregion

        private void OnToggleChanged(CustomGUIToggle toggle, bool value)
        {
            if (!value && toggle == m_selectedToggle)
            {
                m_selectedToggle.isSelected = true;
                return;
            }
            if (toggle == m_selectedToggle)
                return;
            m_selectedToggle = toggle;
            m_selectedToggle.isSelected = true;
            foreach (var otherToggle in m_toggles)
                if (otherToggle != m_selectedToggle)
                    otherToggle.isSelected = false;
        }
    }
}