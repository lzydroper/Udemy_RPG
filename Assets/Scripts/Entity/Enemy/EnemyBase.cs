using System.Collections;
using Core.Utils;
using Entity.Enemy.Skeleton;
using UnityEngine;

namespace Entity.Enemy
{
    public abstract class EnemyBase : EntityBase
    {
        #region Variables

        [Header("Battle")] 
        public float attackDistance = 2;
        public float battleMoveSpeed = 3;
        public float battleDuration = 5;
        public float retreatDistance = 1;
        public Vector2 retreatVelocity = new(5, 3);
        
        [Header("Movement")] 
        public float idleDuration = 2;
        public float moveSpeed = 1.5f;
        private float baseMoveSpeed = 1f;
        public float aniMoveMultiplier => moveSpeed / baseMoveSpeed;
        
        [Header("PlayerDetection")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float playerCheckDistance = 10;
        [SerializeField] private Transform playerCheckPoint;
        public Transform playerTransform;
        
        [Header("Stunned")]
        public float stunDuration = 1;
        public Vector2 stunVelocity = new(7, 7);
        protected bool canBeStunned;

        #endregion

        #region LifecycleMethods
        
        protected override void Awake()
        {
            base.Awake();
            baseMoveSpeed = moveSpeed != 0 ? moveSpeed : 1f;
        }
        
        #endregion

        protected override IEnumerator SlowDownCo(float duration, float slowMultiplier)
        {
            float oriMoveSpeed = baseMoveSpeed;
            float oriBattleMoveSpeed = battleMoveSpeed;
            float oriAniSpeed = ani.speed;
            
            baseMoveSpeed *= slowMultiplier;
            battleMoveSpeed *= slowMultiplier;
            ani.speed *= slowMultiplier;
            
            yield return new WaitForSeconds(duration);
            
            baseMoveSpeed = oriMoveSpeed;
            battleMoveSpeed = oriBattleMoveSpeed;
            ani.speed = oriAniSpeed;
        }

        public override void Death()
        {
            base.Death();
            fsm.ChangeState(EnemyStateType.Death);
        }

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
        
        public void SwitchStunEnable(bool enable) => canBeStunned = enable;
        
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