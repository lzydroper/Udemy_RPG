using System.Collections.Generic;
using SkillSystem.Data;
using SkillSystem.Runtime;
using UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using VInspector;

namespace SkillSystem.UI
{
    public class SkillUITree : MonoBehaviour
    {
        #region Variables

        [Header("Data")]
        [SerializeField] private SkillDataTreeSO skillDataTree;

        [Header("Runtime")]
        [SerializeField] public SkillManager skillManager;

        [Header("UI Roots")]
        [SerializeField] private RectTransform nodeRoot;
        [SerializeField] private RectTransform connectionRoot;

        [Header("Prefabs")]
        [SerializeField] private SkillUINode nodePrefab;
        [SerializeField] private SkillUIConnection connectionPrefab;

        [Header("Layout")]
        [SerializeField] private float nodeSpacing = 100f;

        private UICanvas uiCanvas;
        public SkillUITooltip SkillUITooltip => uiCanvas.skillUITooltip;
        private readonly Dictionary<SkillDataSo, SkillUINode> nodes = new();
        private readonly List<SkillUIConnection> connections = new();

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            uiCanvas = GetComponentInParent<UICanvas>();
            ClearAndCacheEditorTree();
        }

        #endregion

        public bool TryUnlockSkill(SkillDataSo skillData)
        {
            if (skillManager.TryUnlockSkill(skillData))
            {
                foreach (var connection in connections)
                    connection.RefreshState(true);
            }
            return false;
        }
    
        #region Editor Build

#if UNITY_EDITOR

        // 重构技能树
        [Button]
        public void Rebuild()
        {
            if (skillDataTree == null || nodeRoot == null || connectionRoot == null)
                return;
            ClearAndCacheEditorTree();
            CreateEditorNodes();
            CalculateEditorLayout();
            CreateEditorConnections();
            EditorUtility.SetDirty(this);
        }
    
        // 刷新连线情况和相对位置
        [Button]
        public void Refresh()
        {
            ClearAndCacheEditorTree();
            CalculateEditorLayout();
            foreach (var node in nodes.Values)
                node.Refresh();
            foreach (var connection in connections)
                connection.RefreshPosition();
            EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

        // 重置并缓存已存在的技能树
        private void ClearAndCacheEditorTree()
        {
            nodes.Clear();
            connections.Clear();
            foreach (var node in nodeRoot.GetComponentsInChildren<SkillUINode>(true))
            {
                if (node.skillData == null)
                    continue;

                nodes[node.skillData] = node;
            }
            foreach (var conn in connectionRoot.GetComponentsInChildren<SkillUIConnection>(true))
            {
                connections.Add(conn);
            }
        }

        private void CreateEditorNodes()
        {
            foreach (SkillDataSo skill in skillDataTree.skillData)
            {
                if (skill == null || nodes.ContainsKey(skill))
                    continue;
                SkillUINode node = PrefabUtility.InstantiatePrefab(nodePrefab, nodeRoot) as SkillUINode;
                if (node == null)
                    continue;

                node.Bind(skill, this);
                node.Refresh();
                nodes.Add(skill, node);
            }
        }

        private void CalculateEditorLayout()
        {
            // 找根节点
            List<SkillDataSo> roots = FindRootSkills();
            if (roots.Count == 0) return;
            HashSet<SkillDataSo> positioned = new();
            foreach (SkillDataSo root in roots)
            {
                positioned.Add(root);
                CalculateChildrenPosition(root, positioned);
            }
        }
    
        private List<SkillDataSo> FindRootSkills()
        {
            HashSet<SkillDataSo> children = new();

            foreach (SkillConnection connection in skillDataTree.connections)
            {
                if (connection.child != null)
                    children.Add(connection.child);
            }

            List<SkillDataSo> roots = new();

            foreach (SkillDataSo skill in skillDataTree.skillData)
            {
                if (skill == null)
                    continue;

                if (!children.Contains(skill))
                    roots.Add(skill);
            }

            return roots;
        }
    
        private void CalculateChildrenPosition(SkillDataSo parent, HashSet<SkillDataSo> positioned)
        {
            if (!nodes.TryGetValue(parent, out SkillUINode parentNode))
                return;

            foreach (SkillConnection connection in skillDataTree.connections)
            {
                if (connection.parent != parent)
                    continue;

                if (!nodes.TryGetValue(connection.child, out SkillUINode childNode))
                    continue;

                var angle = GetAngleByDirectionType(connection.direction) + connection.rotationOffset;
                Vector2 direction = GetDirectionByAngle(angle);
            
                Vector2 position = parentNode.rectTransform.anchoredPosition + direction * connection.length * nodeSpacing;

                if (positioned.Add(connection.child))
                {
                    childNode.SetPosition(position);
                    CalculateChildrenPosition(connection.child, positioned);
                }
            }
        }

        private void CreateEditorConnections()
        {
            foreach (SkillConnection connection in skillDataTree.connections)
            {
                if (connection.parent == null || connection.child == null)
                    continue;
                if (!nodes.TryGetValue(connection.parent, out SkillUINode parentNode))
                    continue;
                if (!nodes.TryGetValue(connection.child, out SkillUINode childNode))
                    continue;

                SkillUIConnection uiConnection = 
                    PrefabUtility.InstantiatePrefab(connectionPrefab, connectionRoot) as SkillUIConnection;
                if (uiConnection == null)
                    continue;

                uiConnection.name = $"SkillUIConnection - {connection.parent.displayName} -> {connection.child.displayName}";
                uiConnection.Bind(parentNode, childNode, connectionRoot);
                connections.Add(uiConnection);
            }
        }

        private static float GetAngleByDirectionType(ConDirType direction) => direction switch
        {
            ConDirType.Up        => 180f,
            ConDirType.UpRight   => 135f,
            ConDirType.Right     => 90f,
            ConDirType.DownRight => 45f,
            ConDirType.Down      => 0f,
            ConDirType.DownLeft  => -45f,
            ConDirType.Left      => -90f,
            ConDirType.UpLeft    => -135f,
            _                    => 0f,
        };

        private static Vector2 GetDirectionByAngle(float angle)
        {
            float radian = angle * Mathf.Deg2Rad;

            return new Vector2(Mathf.Sin(radian), -Mathf.Cos(radian));
        }

#endif

        #endregion
    }
}