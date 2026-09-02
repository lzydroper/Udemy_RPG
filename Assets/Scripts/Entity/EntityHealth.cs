using Data;
using Entity.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

namespace Entity
{
    public class EntityHealth : MonoBehaviour, IDamageable
    {
#if UNITY_EDITOR
        [Foldout("DebugChoice")]
        [SerializeField] private bool ifShowHealth;
        [SerializeField] private bool ifShowDamageOri;
        [SerializeField] private bool ifShowDamageAft;
        [EndFoldout]
#endif
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
        
        [Header("Health Regen Detail")]
        [SerializeField] private bool canRegen = true;
        [SerializeField] private float reganInterval = 1f;

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
            
            InvokeRepeating(nameof(RegenerateHealth), 0, reganInterval);
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (ifShowHealth)
                Logwin.Log("Health", $"{curHealth}/{_entityStat.GetMaxHealth()}", gameObject.name);
        }
#endif

        #endregion

        private void UpdateHealthBar()
        {
            if (_healthBar == null) return;
            _healthBar.value = curHealth / _entityStat.GetMaxHealth();
        }

        public virtual bool TryTakeDamage(in DamageContext context)
        {
            if (isDead) return false;
            
            if (IsAttackEvasion()) return false;

            // calculate the armor and physic damage
            var armorMitigationPer = _entityStat.GetArmorMitigationPer(context.ArmorReductionPer);
            var physicDamage = context.PhysicDamage * (1 - armorMitigationPer);
            // calculate the resistance and element damage
            var elementResistancePer = _entityStat.GetElementResistancePer(context.ElementType);
            var elementDamage = context.ElementDamage * (1 - elementResistancePer);
            
            var damage = physicDamage + elementDamage;
#if UNITY_EDITOR
            if (ifShowDamageOri)
            {
                Logwin.Log("physicDamageOri", context.PhysicDamage, gameObject.name);
                Logwin.Log("elementDamageOri", context.ElementDamage, gameObject.name);
            }
            if (ifShowDamageAft)
            {
                Logwin.Log("physicDamageAft", physicDamage, gameObject.name);
                Logwin.Log("elementDamageAft", elementDamage, gameObject.name);
            }
#endif
            TakeKnockback(damage, context.DamageDealer);
            ReduceHealth(damage);
            
            return true;
        }

        // TODO: 这个方法不应该在这里
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

        private bool IsAttackEvasion() => Random.Range(0f, 100f) < _entityStat.GetEvasion();
        
        public float GetHealthPercent() => curHealth / _entityStat.GetMaxHealth();
        
        public void SetHealthToPercent(float percent)
        {
            curHealth = _entityStat.GetMaxHealth() * Mathf.Clamp01(percent);
            UpdateHealthBar();
        }

        public void ReduceHealth(float amount)
        {
            if (isDead) return;
            // play vfx
            _entityVFX?.PlayOnDamageVFX();
            // update health value
            curHealth -= amount;
            // update health bar
            UpdateHealthBar();

            if (curHealth <= 0f)
            {
                Die();
            }
        }

        public void IncreaseHealth(float amount)
        {
            if (isDead) return;
            // play vfx
            // update health value
            float maxHealth = _entityStat.GetMaxHealth();
            curHealth = Mathf.Min(curHealth + amount, maxHealth);
            // update health bar
            UpdateHealthBar();
        }

        private void RegenerateHealth()
        {
            if (!canRegen || isDead) return;
            float amount = _entityStat.healthRegen.GetValue();
            IncreaseHealth(amount);
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