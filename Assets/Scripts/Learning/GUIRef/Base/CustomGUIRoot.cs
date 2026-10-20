using UnityEngine;

namespace Learning.GUIRef.Base
{
    [ExecuteAlways]
    public class CustomGUIRoot : MonoBehaviour
    {
        #region Variables

        private ICustomGUIDrawable[] m_drawables;

        #endregion

        #region LifecycleMethods

        private void Start()
        {
            m_drawables = GetComponentsInChildren<ICustomGUIDrawable>();
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
                m_drawables = GetComponentsInChildren<ICustomGUIDrawable>();
            // Debug.Log(m_drawables.Length);
            foreach (ICustomGUIDrawable drawable in m_drawables)
                drawable.Draw();
        }

        #endregion
    }
}