using Data;
using Entity;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjSwordSpin : SkillObjSword
    {
        #region Variables

        private float attackTimer;
        private float attackInterval;

        #endregion

        #region LifecycleMethods

        protected override void Update()
        {
            HandleAttack();
            HandleStop();
            HandleComeback();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            rb.simulated = false;
        }

        #endregion

        public override void SetupSword(SkillSwordThrow skill, Vector2 velocity, Transform player, 
            EntityStat entityStat, EntityVFX entityVFX, DamageScaleData damageScale)
        {
            base.SetupSword(skill, velocity, player, entityStat, entityVFX, damageScale);
            ani.SetBool("isSpinning", true);
            attackInterval = 1 / skill.attacksPerSecond;
            Invoke(nameof(GetSwordBackToPlayer), skill.maxSpinDuration);
        }

        private void HandleAttack()
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                DamageInRadius(transform, 1);
                attackTimer = attackInterval;
            }
        }

        private void HandleStop()
        {
            var disToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (disToPlayer > skill.maxDistance && rb.simulated)
                rb.simulated = false;
        }
    }
}