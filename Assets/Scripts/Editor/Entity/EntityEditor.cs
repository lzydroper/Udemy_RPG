using UnityEditor;
using UnityEngine;

namespace Entity
{
    [CustomEditor(typeof(EntityBase), true)]
    public class EntityEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var entity = (EntityBase)target;
            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.LabelField("PreState",
                    (entity?.PreStateKey != null && entity.PreStateKey != "0") ?
                        entity.PreStateKey : "(null)");
                EditorGUILayout.LabelField("CurState",
                    (entity?.CurStateKey != null && entity.CurStateKey != "0") ?
                        entity.CurStateKey : "(null)");
            }

            // Play 模式下持续刷新，否则值变化了 Inspector 不会自动重绘
            if (Application.isPlaying)
            {
                Repaint();
            }
        }
    }
}