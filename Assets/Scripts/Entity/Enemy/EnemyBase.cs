using Core.Utils;
using Entity.Enemy.Skeleton;
using UnityEngine;

namespace Entity.Enemy
{
    public abstract class EnemyBase : EntityBase
    {
        #region Variables

        [Header("Battle")] 
        public float attackDistance;
        public float attackMoveSpeed;
        public float battleDuration;
        public float retreatDistance;
        public Vector2 retreatVelocity;
        
        [Header("Movement")] 
        public float idleDuration;
        public float moveSpeed;
        private float baseMoveSpeed = 1f;
        public float aniMoveMultiplier => moveSpeed / baseMoveSpeed;
        
        [Header("PlayerDetection")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float playerCheckDistance;
        [SerializeField] private Transform playerCheckPoint;
        public Transform playerTransform;

        #endregion

        #region LifecycleMethods
        
        protected override void Awake()
        {
            base.Awake();
            baseMoveSpeed = moveSpeed != 0 ? moveSpeed : 1f;
        }
        
        #endregion

        public RaycastHit2D PlayerDetected()
        {
            RaycastHit2D hit = Physics2D.Raycast(
                playerCheckPoint.position, Vector2.right * facingDir, 
                         playerCheckDistance, playerLayer | groundLayer);
            if (hit.collider == null || !hit.collider.IsInLayerMask(playerLayer))
            {
                return default;
            }
            return hit;
        }

        public void TryEnterBattleState(Transform player)
        {
            if (fsm.CurStateKey is SkeletonStateType.Battle or SkeletonStateType.Attack)
                return;
            playerTransform = player;
            fsm.ChangeState(SkeletonStateType.Battle);
        }

        public Transform GetPlayerTransform()
        {
            if (!playerTransform)
                playerTransform = PlayerDetected().transform;
            return playerTransform;
        }
        
#if UNITY_EDITOR
        [ContextMenu("TestToAttackState")]
        private void TestToAttackState() => fsm.ChangeState(EnemyStateType.Attack);
        
        #region GizmosMethods

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(playerCheckPoint.position, 
                playerCheckPoint.position + Vector3.right * facingDir * playerCheckDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerCheckPoint.position, 
                playerCheckPoint.position + Vector3.right * facingDir * attackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerCheckPoint.position, 
                playerCheckPoint.position + Vector3.right * facingDir * retreatDistance);
        }
        
        #endregion
#endif
    }
}