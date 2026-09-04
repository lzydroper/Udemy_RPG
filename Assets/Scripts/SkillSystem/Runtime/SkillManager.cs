using System.Collections.Generic;
using Entity.Player;
using SkillSystem.Data;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private SkillDataTreeSO skillDataTree;
        [SerializeField] private int skillPoints;
        private readonly Dictionary<SkillDataSo, bool> unlocks = new();
        private readonly Dictionary<SkillType, SkillBase> skills = new();
        [HideInInspector] public Player player;
        [SerializeField] private GameObject skillHolder;

        #endregion

        #region SkillPointsMethods
    
        public bool EnoughSkillPoints(int cost) => skillPoints >= cost;
        public void RemoveSkillPoints(int cost) => skillPoints -= cost;
        public void IncreaseSkillPoints(int cnt) => skillPoints += cnt;

        #endregion
    
        #region LifeCycleMethods

        public void Awake()
        {
            GetAllSkills();
            // build dict
            foreach (var skillData in skillDataTree.skillData)
            {
                unlocks.Add(skillData, false);
            }
            // build default unlock
            foreach (var skillData in skillDataTree.skillData)
            {
                if (skillData.isUnlockedDefault)
                    TryUnlockSkill(skillData);
            }
            
            player = GetComponent<Player>();
        }
    
        #endregion
    
        #region GetSkillSituationMethods

        // 即不冲突，也未解锁
        public bool IsSkillAvailable(SkillDataSo skillData) 
            => !IsSkillUnlocked(skillData) && !IsSkillConflict(skillData);

        public bool IsSkillUnlocked(SkillDataSo skillData)
        {
            if (!unlocks.TryGetValue(skillData, out var isUnlock) || !isUnlock)
                return false;
            return true;
        }

        /// <summary>
        /// 判断指定技能是否为冲突状态，任一conflict已解锁即为冲突状态
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public bool IsSkillConflict(SkillDataSo skillData)
        {
            // 技能不存在，不存在冲突可能性
            if (!unlocks.ContainsKey(skillData))
                return false;
            // 技能冲突都未解锁
            foreach (var conflictData in skillData.conflicts)
            {
                if (!unlocks.TryGetValue(conflictData, out var isUnlock) || isUnlock)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断指定技能是否可解锁，需满足所有pre已解锁、所有conflict未解锁，技能消耗满足
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns></returns>
        public bool IsSkillCanUnlock(SkillDataSo skillData)
        {
            // 技能存在，且未解锁
            if (!unlocks.TryGetValue(skillData, out var isUnlock) || isUnlock)
                return false;
            // 技能点足够
            if (!EnoughSkillPoints(skillData.cost))
                return false;
            // 技能前置都已解锁
            foreach (var prereqData in skillData.prerequisites)
            {
                if (!unlocks.TryGetValue(prereqData, out isUnlock) || !isUnlock)
                    return false;
            }
            // 技能冲突都未解锁
            foreach (var conflictData in skillData.conflicts)
            {
                if (!unlocks.TryGetValue(conflictData, out isUnlock) || isUnlock)
                    return false;
            }

            return true;
        }
    
        #endregion

        public SkillBase GetSkillByType(SkillType skillType)
        {
            return skills.GetValueOrDefault(skillType);
        }

        public T GetSkillByType<T>(SkillType skillType) where T : SkillBase
        {
            return skills.GetValueOrDefault(skillType) as T;
        }

        private void GetAllSkills()
        {
            foreach (var skill in skillHolder.GetComponentsInChildren<SkillBase>())
            {
                skills.Add(skill.skillType, skill);
            }
        }

        public bool TryUnlockSkill(SkillDataSo skillData)
        {
            if (!IsSkillCanUnlock(skillData)) 
                return false;
            // 扣除技能点
            RemoveSkillPoints(skillData.cost);
            // 标记解锁
            unlocks[skillData] = true;
            skills[skillData.skillType].SetUpgrade(skillData.upgradeData);
            return true;
        }

        public void RefundAllSkills()
        {
            // 把所有非默认解锁的重置解锁状态
            foreach (var skillData in unlocks.Keys)
            {
                if (unlocks[skillData] && !skillData.isUnlockedDefault)
                {
                    unlocks[skillData] = false;
                    IncreaseSkillPoints(skillData.cost);
                    // TODO: 重置技能使用层面解锁状态
                }
            }
        }
    }
}