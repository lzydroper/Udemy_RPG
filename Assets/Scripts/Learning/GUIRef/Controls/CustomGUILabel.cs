using Learning.GUIRef.Base;
using UnityEngine;

namespace Learning.GUIRef.Controls
{
    public class CustomGUILabel : CustomGUIControl
    {
        protected override void DrawStyleOn()
        {
            GUI.Label(position.rect, content, style);
        }

        protected override void DrawStyleOff()
        {
            GUI.Label(position.rect, content);
        }
    }
}