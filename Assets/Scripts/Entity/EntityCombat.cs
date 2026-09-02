using Data;
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

        [SerializeField] private DamageScaleData basicAttackScale;
        
        [Header("TargetDetection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float targetCheckRadius = 1;
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

                ElementalEffectData effectData = new ElementalEffectData(_entityStat, basicAttackScale);
                
                var context = new DamageContext(transform, _entityStat, basicAttackScale);
                
                if (damageable.TryTakeDamage(context))
                {
                    if (context.ElementType != ElementType.None) 
                        target.GetComponent<EntityStatusHandler>()?.ApplyElementEffect(context.ElementType, effectData);
                    _entityVFX?.CreateOnHitVFX(target.transform, context.IsCritical, context.ElementType);
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