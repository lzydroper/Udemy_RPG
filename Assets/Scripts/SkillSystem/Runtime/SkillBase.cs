using System;
using Data;
using SkillSystem.Data;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public abstract class SkillBase : MonoBehaviour
    {
        [SerializeField] public SkillType skillType;
        public SkillManager skillManager;
        [SerializeField] protected SkillUpgradeType upgradeType;
        public DamageScaleData damageScale;
        protected float cooldown;
        private float lastTimeUsed;

        #region LifeCycleMethods

        protected virtual void Awake()
        {
            skillManager = GetComponentInParent<SkillManager>();
        }

        #endregion

        public virtual void UseSkill() { }

        public void SetUpgrade(UpgradeData upgradeData)
        {
            upgradeType = upgradeData.upgradeType;
            cooldown = upgradeData.cooldown;
            damageScale = upgradeData.damageScaleData;
            lastTimeUsed = Time.time;
        }

        public virtual bool IsCanUseSkill()
        {
            if (IsCooldown())
                return false;

            if (upgradeType is SkillUpgradeType.None)
                return false;
            
            return true;
        }
        
        protected bool IsCooldown() => Time.time - lastTimeUsed < cooldown;
        public void SetOnCooldown() => lastTimeUsed = Time.time;
    }
}