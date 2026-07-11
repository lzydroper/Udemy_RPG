using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Entity
{
    [CustomEditor(typeof(EntityBase), true)]
    public class EntityEditor : Editor
    {
        private Editor _vInspectorEditor;

        private void OnEnable()
        {
            var vType = System.Type.GetType("VInspector.ScriptEditor, VInspector");
            if (vType != null)
                _vInspectorEditor = CreateEditor(targets, vType);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            var entity = (EntityBase)target;
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

            _vInspectorEditor.OnInspectorGUI();

            if (Application.isPlaying) Repaint();
        }

        private void OnDisable()
        {
            if (_vInspectorEditor != null) DestroyImmediate(_vInspectorEditor);
        }
    }
}