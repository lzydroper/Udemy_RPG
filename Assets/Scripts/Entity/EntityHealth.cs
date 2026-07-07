using Core.Utils;
using UnityEngine;

namespace Entity
{
    public class EntityHealth : MonoBehaviour
    {
        #region Variables

        private EntityVFX _entityVFX;
        private EntityBase _entityBase;
        
        [SerializeField] protected float maxHealth = 100f;
        [SerializeField] protected float curHealth = 100f;
        protected bool isDead = false;
        
        [Header("OnDamageKnockback")]
        [SerializeField] private Vector2 knockbackVelocity;
        [SerializeField] private float knockbackDuration = 0.5f;

        #endregion
        
        #region LifecycleMethods

        protected virtual void Awake()
        {
            _entityVFX = GetComponent<EntityVFX>();
            _entityBase = GetComponent<EntityBase>();
        }

        #endregion

        public virtual void TakeDamage(float damage, Transform damageDealer)
        {
            if (isDead) return;
            
            _entityBase.ReceiveKnockback(CalKnockbackVelocity(damageDealer), knockbackDuration);
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

        private Vector2 CalKnockbackVelocity(Transform dealer)
        {
            int direction = dealer.position.x > transform.position.x ? -1 : 1;
            Vector2 knockback = knockbackVelocity;
            knockback.x *= direction;
            return knockback;
        }
    }
}