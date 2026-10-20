using Learning.GUIRef.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Learning.GUIRef.Controls
{
    public class CustomGUIButton : CustomGUIControl
    {
        public UnityEvent onClick = new();
        
        protected override void DrawStyleOn()
        {
            if(GUI.Button(position.rect, content, style))
                onClick.Invoke();
        }

        protected override void DrawStyleOff()
        {
            if(GUI.Button(position.rect, content))
                onClick.Invoke();
        }
    }
}