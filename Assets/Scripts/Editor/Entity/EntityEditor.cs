using UnityEditor;
using UnityEngine;

namespace Entity
{
    [CustomEditor(typeof(EntityBase), true)]
    public class EntityEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var entity = (EntityBase)target;
            
            serializedObject.Update();

            // 1. 手动绘制最顶部的 Script 字段
            var scriptProp = serializedObject.FindProperty("m_Script");
            if (scriptProp != null)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.PropertyField(scriptProp);
                }
            }
            
            EditorGUILayout.Space();
            // 2. 绘制状态字段（在 Script 下方，其他属性上方）
            using (new EditorGUI.DisabledScope(true))
            {
                string preState = string.IsNullOrEmpty(entity?.PreStateKey) || entity.PreStateKey == "0" 
                    ? "(null)" 
                    : entity.PreStateKey;
                string curState = string.IsNullOrEmpty(entity?.CurStateKey) || entity.CurStateKey == "0" 
                    ? "(null)" 
                    : entity.CurStateKey;
                
                EditorGUILayout.LabelField($"PreState: {preState}");
                EditorGUILayout.LabelField($"CurState: {curState}");
            }
            
            // EditorGUILayout.Space();

            // 3. 排除 Script 字段后，绘制其余默认的序列化字段
            DrawPropertiesExcluding(serializedObject, "m_Script");

            // 4. 绘制带有 [Button] 特性的方法
            DrawButtons(entity);

            serializedObject.ApplyModifiedProperties();

            // Play 模式下持续刷新，否则值变化了 Inspector 不会自动重绘
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        private void DrawButtons(EntityBase entity)
        {
            var methods = entity.GetType().GetMethods(
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Static | 
                System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var buttonAttr = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<NaughtyAttributes.ButtonAttribute>(method);
                if (buttonAttr != null)
                {
                    // 安全获取按钮显示的文本
                    string buttonText = null;
                    var textProp = buttonAttr.GetType().GetProperty("Text", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (textProp != null)
                    {
                        buttonText = textProp.GetValue(buttonAttr) as string;
                    }
                    else
                    {
                        var textField = buttonAttr.GetType().GetField("text", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (textField != null)
                        {
                            buttonText = textField.GetValue(buttonAttr) as string;
                        }
                    }

                    if (string.IsNullOrEmpty(buttonText))
                    {
                        buttonText = ObjectNames.NicifyVariableName(method.Name);
                    }

                    if (GUILayout.Button(buttonText))
                    {
                        method.Invoke(entity, null);
                    }
                }
            }
        }
    }
}