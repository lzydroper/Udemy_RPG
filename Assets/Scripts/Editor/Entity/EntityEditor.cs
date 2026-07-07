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

            serializedObject.ApplyModifiedProperties();

            // Play 模式下持续刷新，否则值变化了 Inspector 不会自动重绘
            if (Application.isPlaying)
            {
                Repaint();
            }
        }
    }
}