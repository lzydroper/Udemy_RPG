using Entity.Interfaces;
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
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private Transform targetCheckPoint;

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

                // who combat is to use the reduction value
                var armorReductionPer = _entityStat.GetArmorReductionPer();
                var targetEntityStat = target.GetComponent<EntityStat>();
                var armorMitigationPer = targetEntityStat ? 
                    targetEntityStat.GetArmorMitigationPer(armorReductionPer) : 0;
                var armorMitigationMultiplier = 1 - armorMitigationPer;
                var physicalDamage = _entityStat.GetPhysicalDamage(out var isCritical);
                var damage = physicalDamage * armorMitigationMultiplier;
                if (damageable.TryTakeDamage(damage, transform))
                {
                    _entityVFX.CreateOnHitVFX(target.transform, isCritical);
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