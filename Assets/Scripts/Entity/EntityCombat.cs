using Entity.Interfaces;
using UnityEngine;

namespace Entity
{
    public class EntityCombat : MonoBehaviour
    {
        #region Variables
        
        private EntityVFX _entityVFX;

        public float damage;
        [Header("TargetDetection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private Transform targetCheckPoint;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            _entityVFX = GetComponent<EntityVFX>();
        }

        #endregion

        public void PerformAttack()
        {
            foreach (var target in GetDetectTargets())
            {
                var damageable = target.GetComponent<IDamageable>();
                if (damageable == null) continue;

                if (damageable.TryTakeDamage(damage, transform))
                {
                    _entityVFX.CreateOnHitVFX(target.transform);
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