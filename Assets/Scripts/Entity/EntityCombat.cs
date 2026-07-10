using Entity.Interfaces;
using UnityEngine;

namespace Entity
{
    public class EntityCombat : MonoBehaviour
    {
        #region Variables

        public float damage;
        [Header("TargetDetection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private Transform targetCheckPoint;

        #endregion

        public void PerformAttack()
        {
            foreach (var target in GetDetectTargets())
            {
                target.GetComponent<IDamageable>()?.TakeDamage(damage, transform);
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