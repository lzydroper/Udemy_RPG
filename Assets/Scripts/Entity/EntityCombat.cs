using Entity.Interfaces;
using Stats;
using UnityEngine;

namespace Entity
{
    public class EntityCombat : MonoBehaviour
    {
        #region Variables
        
        private EntityVFX _entityVFX;
        private EntityStat _entityStat;
        
        [Header("TargetDetection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float targetCheckRadius = 1;
        [SerializeField] private Transform targetCheckPoint;
        [Header("StatusEffectDetails")]
        [SerializeField] private float defaultDuration = 3;
        [SerializeField] private float slowPercentage = 0.5f;
        [SerializeField] private float chargeBuildUp = 0.4f;
        
        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            _entityVFX = GetComponent<EntityVFX>();
            _entityStat = GetComponent<EntityStat>();
        }

        #endregion

        public void PerformAttack()
        {
            foreach (var target in GetDetectTargets())
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable == null) continue;

                var physicDamage = _entityStat.GetPhysicDamage(out var isCritical);
                var elementDamage = _entityStat.GetElementDamage(out var elementType, .5f);
                var armorReductionPer = _entityStat.GetArmorReductionPer();
                var context = new DamageContext(transform, physicDamage, elementDamage, elementType, armorReductionPer);
                if (damageable.TryTakeDamage(context))
                {
                    ApplyElementEffect(target.transform, elementType);
                    _entityVFX?.UpdateOnHitColor(elementType);
                    _entityVFX?.CreateOnHitVFX(target.transform, isCritical);
                }
            }
        }

        public void ApplyElementEffect(Transform target, ElementType type, float scale = 1f)
        {
            if (type == ElementType.None) return;
            var handler = target.GetComponent<EntityStatusHandler>();
            if (handler != null && handler.CanBeAppliedElementEffect(type))
            {
                if (type == ElementType.Ice)
                {
                    handler.ApplyIceEffect(defaultDuration, (1 - slowPercentage) * scale);
                }
                if (type == ElementType.Fire)
                {
                    handler.ApplyFireEffect(defaultDuration, _entityStat.fireDamage.GetValue() * scale);
                }
                if (type == ElementType.Lightning)
                {
                    handler.ApplyLightningEffect(defaultDuration, 
                        _entityStat.lightningDamage.GetValue() * scale,
                        chargeBuildUp);
                }
            }
        }

        protected Collider2D[] GetDetectTargets()
        {
            return Physics2D.OverlapCircleAll(targetCheckPoint.position, targetCheckRadius, targetLayer);
        }
        
#if UNITY_EDITOR
        #region GizmosMethods
        
        protected void OnDrawGizmos/*Selected*/()
        {
            Gizmos.DrawWireSphere(targetCheckPoint.position, targetCheckRadius);
        }
        
        #endregion
#endif
    }
}