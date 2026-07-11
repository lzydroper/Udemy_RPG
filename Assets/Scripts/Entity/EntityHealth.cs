using Entity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    public class EntityHealth : MonoBehaviour, IDamageable
    {
        #region Variables

        private EntityVFX _entityVFX;
        private EntityBase _entityBase;
        private Slider _healthBar;
        
        [SerializeField] protected float maxHealth = 100f;
        protected float curHealth = 100f;
        protected bool isDead = false;
        
        [Header("OnDamageKnockback")]
        [SerializeField] private Vector2 knockbackVelocity = new(1.5f, 2.5f);
        [SerializeField] private float knockbackDuration = 0.2f;

        [Header("OnHeavyDamageKnockback")] 
        [SerializeField] private float heavyDamageThreshold = 0.3f;
        [SerializeField] private Vector2 heavyKnockbackVelocity = new(7f, 7f);
        [SerializeField] private float heavyKnockbackDuration = 0.5f;

        #endregion
        
        #region LifecycleMethods

        protected virtual void Awake()
        {
            _entityVFX = GetComponent<EntityVFX>();
            _entityBase = GetComponent<EntityBase>();
            _healthBar = GetComponentInChildren<Slider>();
            curHealth = maxHealth;
            UpdateHealthBar();
        }

        #endregion

        private void UpdateHealthBar()
        {
            if (_healthBar == null) return;
            _healthBar.value = curHealth / maxHealth;
        }

        public virtual void TakeDamage(float damage, Transform damageDealer)
        {
            if (isDead) return;

            if (damage > heavyDamageThreshold * maxHealth)
            {
                _entityBase.ReceiveKnockback(CalKnockbackVelocity(heavyKnockbackVelocity, damageDealer), heavyKnockbackDuration);
            }
            else
            {
                _entityBase.ReceiveKnockback(CalKnockbackVelocity(knockbackVelocity, damageDealer), knockbackDuration);
            }
            _entityVFX?.PlayOnDamageVFX();
            ReduceHealth(damage);
        }

        protected void ReduceHealth(float amount)
        {
            curHealth -= amount;
            UpdateHealthBar();

            if (curHealth <= 0f)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            isDead = true;
            _entityBase?.Death();
        }

        private Vector2 CalKnockbackVelocity(Vector2 knockbackVe,Transform dealer)
        {
            int direction = dealer.position.x > transform.position.x ? -1 : 1;
            Vector2 knockback = knockbackVe;
            knockback.x *= direction;
            return knockback;
        }
    }
}