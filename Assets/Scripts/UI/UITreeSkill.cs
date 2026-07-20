using UnityEngine;
using VInspector;

namespace UI
{
    public class UITreeSkill : MonoBehaviour
    {
        #region Variables

        [SerializeField] private int skillPoints;
        [SerializeField] private UITreeNodeConHandler[] conHandlers;

        #endregion

        public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
        public void RemoveSkillPoints(int cost) => skillPoints -= cost;
        public void IncreaseSkillPoints(int cnt) => skillPoints += cnt;

        [Button]
        public void RefundAllSkill()
        {
            var nodes = GetComponentsInChildren<UITreeNode>();

            foreach (var node in nodes)
            {
                node.Refund();
            }
        }
        
        [Button]
        public void UpdateAllCon()
        {
            foreach (var con in conHandlers)
            {
                con.UpdateAllCon();
            }
        }
    }
}