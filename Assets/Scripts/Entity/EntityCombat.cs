using UnityEngine;

namespace Entity
{
    public class EntityCombat : MonoBehaviour
    {
        #region Variables

        [Header("TargetDetection")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float targetCheckRadius;
        [SerializeField] private Transform targetCheckPoint;

        #endregion

        #region LifecycleMethods

        

        #endregion

        public void PerformAttack()
        {
            foreach (var target in Physics2D.OverlapCircleAll(transform.position, targetCheckRadius, targetLayer))
            {
                Debug.Log(target.name);
            }
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