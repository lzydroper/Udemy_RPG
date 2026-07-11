using Core.Utils;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyDatabase))]
public class KeyDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制默认Inspector
        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        GUI.backgroundColor = new Color(0.4f, 0.8f, 1f);

        if (GUILayout.Button("Generate", GUILayout.Height(35)))
        {
            var database = (KeyDatabase)target;

            KeyGenerator.Generate(database);

            // 标记Database已保存
            EditorUtility.SetDirty(database);

            Debug.Log("Generate Finished.");
        }

        GUI.backgroundColor = Color.white;
    }
}