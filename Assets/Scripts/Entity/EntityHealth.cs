using Entity.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Entity
{
    public class EntityHealth : MonoBehaviour, IDamageable
    {
        #region Variables

        private EntityBase _entityBase;
        private EntityVFX _entityVFX;
        private EntityStat _entityStat;
        private Slider _healthBar;
        
        public float curHealth = 100f;
        public bool isDead = false;
        
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
            _entityBase = GetComponent<EntityBase>();
            _entityVFX = GetComponent<EntityVFX>();
            _entityStat = GetComponent<EntityStat>();
            _healthBar = GetComponentInChildren<Slider>();
            curHealth = _entityStat.GetMaxHealth();
            UpdateHealthBar();
        }

        #endregion

        private void UpdateHealthBar()
        {
            if (_healthBar == null) return;
            _healthBar.value = curHealth / _entityStat.GetMaxHealth();
        }

        public virtual bool TryTakeDamage(in DamageContext context)
        {
            if (isDead) return false;
            
            if (AttackEvasion()) return false;

            // calculate the armor and physic damage
            var armorMitigationPer = _entityStat.GetArmorMitigationPer(context.ArmorReductionPer);
            var physicDamage = context.PhysicDamage * (1 - armorMitigationPer);
            // calculate the resistance and element damage
            var elementResistancePer = _entityStat.GetElementResistancePer(context.ElementType);
            var elementDamage = context.ElementDamage * (1 - elementResistancePer);
            
            var damage = physicDamage + elementDamage;
            Debug.Log($"physicDamageOri: {context.PhysicDamage}, physicDamageAft: {physicDamage}\n" +
                      $"elementDamageOri: {context.ElementDamage}, elementDamageAft: {elementDamage}");
            
            TakeKnockback(damage, context.DamageDealer);
            ReduceHealth(damage);
            
            return true;
        }

        private void TakeKnockback(float damage, Transform damageDealer)
        {
            if (damage > heavyDamageThreshold * _entityStat.GetMaxHealth())
            {
                _entityBase.ReceiveKnockback(
                    CalKnockbackVelocity(heavyKnockbackVelocity, damageDealer), 
                    heavyKnockbackDuration);
            }
            else
            {
                _entityBase.ReceiveKnockback(
                    CalKnockbackVelocity(knockbackVelocity, damageDealer), 
                    knockbackDuration);
            }
        }

        private bool AttackEvasion() => Random.Range(0f, 100f) < _entityStat.GetEvasion();

        protected void ReduceHealth(float amount)
        {
            _entityVFX?.PlayOnDamageVFX();
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