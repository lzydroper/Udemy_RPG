using UnityEditor;
using UnityEngine;

namespace Learning.GUIRef.Base
{
    public enum E_Style_OnOff
    {
        On,
        Off,
    }
    [ExecuteAlways]
    public abstract class CustomGUIControl : MonoBehaviour, ICustomGUIDrawable
    {
        #region Variables

        [SerializeField] private GameObject rootPrefab;
        [SerializeField] protected CustomGUIPosition position = new();
        [SerializeField] protected GUIContent content;
        [SerializeField] protected GUIStyle style;
        [SerializeField] private E_Style_OnOff style_onoff = E_Style_OnOff.Off;

        #endregion

        #region LifecycleMethods

#if UNITY_EDITOR

        private void Start()
        {
            if (!Application.isPlaying)
            {   // 使用 delayCall 延迟到当前帧层级更新后安全执行
                EditorApplication.delayCall += EnsureParentStructure;
            }
        }

        private void EnsureParentStructure()
        {
            if (this == null || gameObject == null)
                return;
            if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
                return;
            if (GetComponentInParent<CustomGUIRoot>() != null)
                return;
            if (rootPrefab == null)
                return;
            GameObject parent = PrefabUtility.InstantiatePrefab(rootPrefab, gameObject.scene) as GameObject;
            if (parent == null)
                return;
            // 记录撤销操作，保证 Ctrl+Z 功能正常
            Undo.RegisterCreatedObjectUndo(parent, "Auto Instantiate Parent");

            // 继承位置并挂载
            // 将父节点对齐到当前物体位置，便于观察
            parent.transform.position = transform.position;
            parent.transform.rotation = Quaternion.identity;

            // 记录子物体的父级修改到 Undo 栈
            Undo.SetTransformParent(transform, parent.transform, "Auto Set Parent");

            // 标记场景修改，让用户能保存（Ctrl+S）
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

#endif
        
        #endregion
        
        public virtual void Draw()
        {
            if (style_onoff == E_Style_OnOff.On)
                DrawStyleOn();
            else if (style_onoff == E_Style_OnOff.Off)
                DrawStyleOff();
        }

        protected abstract void DrawStyleOn();

        protected abstract void DrawStyleOff();
    }
}