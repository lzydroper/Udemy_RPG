using System;
using Data;
using UnityEngine;

namespace SkillSystem.Data
{
    [CreateAssetMenu(fileName = "SkillDataSO", menuName = "Data/Skill/SkillDataSO", order = 0)]
    public class SkillDataSo : ScriptableObject
    {
        [Header("Skill Definition")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public int cost = 1;
        public bool isUnlockedDefault = false;
        [Header("Skill Relation")] 
        public SkillDataSo[] prerequisites;
        public SkillDataSo[] conflicts;
        [Header("Skill Upgrades")]
        public SkillType skillType;
        public UpgradeData upgradeData;
    }
    
    [Serializable]
    public class UpgradeData
    {
        public SkillUpgradeType upgradeType;
        public float cooldown;
        public DamageScaleData damageScaleData;
    }
}