using System;
using Core.Pool;
using Core.Utils;
using Entity.Enemy;
using Generated;
using UnityEngine;

namespace Skills
{
    public class SkillObjShard : SkillObjBase
    {
        public void SetupShard(float delay)
        {
            Invoke(nameof(Explore), delay);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBase>(out _))
            {
                Explore();
            }
        }

        private void Explore()
        {
            CancelInvoke(nameof(Explore));
            GameManager.Pool.RentAsync(Keys.Pool.TimeShardVFX, position: transform.position);
            DamageInRadius(transform, checkRadius);
            this.Return();
        }
    }
}