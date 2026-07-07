using Core.Utils;
using UnityEngine;

namespace Entity
{
    public class EntityHealth : MonoBehaviour
    {
        #region Variables

        private EntityVFX _entityVFX;
        
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float curHealth = 100f;
        protected bool isDead = false;

        #endregion
        
        #region LifecycleMethods

        protected virtual void Awake()
        {
            _entityVFX = GetComponent<EntityVFX>();
        }

        #endregion

        public virtual void TakeDamage(float damage, Transform damageDealer)
        {
            if (isDead) return;
            
            _entityVFX?.PlayOnDamageVFX();
            ReduceHealth(damage);
        }

        protected void ReduceHealth(float amount)
        {
            curHealth -= amount;

            if (curHealth <= 0f)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            isDead = true;
        }
    }
}