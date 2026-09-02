using Core.Utils;
using Data;
using Entity;
using Entity.Enemy;
using Generated;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjShard : SkillObjBase
    {
        #region Variables

        private Transform target;
        private float moveSpeed;
        private SkillTimeShard skill;

        #endregion

        #region Lifecycle Methods
        
        private void Update()
        {
            if (!target) return;
            transform.position = Vector3.MoveTowards(
                transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        #endregion

        public void MoveTowardClosest(float speed)
        {
            target = GetClosestTarget(transform, closestCheckR);
            moveSpeed = speed;
        }

        public void SetupShard(SkillTimeShard skill, float detonateDelay, EntityStat entityStat, EntityVFX entityVFX, 
            DamageScaleData damageScale)
        {
            this.skill = skill;
            this.entityStat = entityStat;
            this.entityVFX = entityVFX;
            this.damageScale = damageScale;
            Invoke(nameof(Explode), detonateDelay);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBase>(out _))
            {
                Explode();
            }
        }

        public void Explode()
        {
            skill.OnShardExplode();
            CancelInvoke(nameof(Explode));
            DamageInRadius(transform, checkRadius, out var elementType);
            GameManager.Pool.TryRent(Keys.Pool.TimeShardVFX, out var shard, position: transform.position);
            var sr = shard.GetComponentInChildren<SpriteRenderer>();
            sr.color = entityVFX.GetColorByType(elementType);
            sr.ReturnAsync();
            this.Return();
        }
    }
}