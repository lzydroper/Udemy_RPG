using Learning.GUIRef.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Learning.GUIRef.Controls
{
    public class CustomGUIToggle : CustomGUIControl
    {
        public UnityEvent<bool> onValueChanged = new();
        public bool isSelected;
        protected override void DrawStyleOn()
        {
            bool value = GUI.Toggle(position.rect, isSelected, content, style);
            HandleToggleChanged(value);
        }

        protected override void DrawStyleOff()
        {
            bool value = GUI.Toggle(position.rect, isSelected, content);
            HandleToggleChanged(value);
        }

        private void HandleToggleChanged(bool value)
        {
            if (isSelected == value) 
                return;
            isSelected = value;
            onValueChanged.Invoke(value);
        }
    }
}