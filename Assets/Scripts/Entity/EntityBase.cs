using System.Collections;
using Core.State;
using Core.Utils;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine = Core.State.StateMachine;

namespace Entity
{
    public abstract class EntityBase : MonoBehaviour, IStateOwner
    {
        #region IStateOwner
        
        public Animator ani { get; private set; }
        public Rigidbody2D rb { get; private set; }

        #endregion

        #region Variables
        
        // state machine
        public string CurStateKey => fsm?.CurStateKey;
        public string PreStateKey => fsm?.PreStateKey;
        protected StateMachine fsm;

        // move
        protected bool isFacingRight = true;
        public float facingDir { get; private set; } = 1;
        // collision detect
        [Header("collision detection")]
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Transform wallCheckTopPoint;
        [SerializeField] private Transform wallCheckBottomPoint;
        public bool GroundDetected { get; private set; }
        public bool WallDetected { get; private set; }
        private bool needSecondaryWallCheck;

        private bool isKnocked;
        private Coroutine knockbackCoroutine;

        #endregion
        
        #region InheritMethods
        
        protected virtual void FetchComponents()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody2D>();
        }

        protected virtual void BuildStateMachine()
        {
            fsm = new StateMachine();
        }
        
        #endregion

        #region LifecycleMethods

        protected virtual void Awake()
        {
            if (wallCheckBottomPoint == null)
            {
                needSecondaryWallCheck = false;
            }
            FetchComponents();
            BuildStateMachine();
        }

        protected virtual void Update()
        {
            HandleCollisionDetect();
            fsm.OnUpdate();
        }

        #endregion

        public virtual void Death() { }

        public void ReceiveKnockback(Vector2 knockbackVelocity, float knockbackDuration) 
            => this.RestartCoroutine(ref knockbackCoroutine, KnockbackCo(knockbackVelocity, knockbackDuration));
        
        private IEnumerator KnockbackCo(Vector2 knockbackVelocity, float knockbackDuration)
        {
            isKnocked = true;
            rb.linearVelocity = knockbackVelocity;
            
            yield return new WaitForSeconds(knockbackDuration);
            
            rb.linearVelocity = Vector2.zero;
            isKnocked = false;
        }

        private void HandleCollisionDetect()
        {
            GroundDetected = Physics2D.Raycast(
                groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
            if (needSecondaryWallCheck)
            {
                WallDetected = Physics2D.Raycast(
                               wallCheckTopPoint.position, Vector2.right * facingDir, wallCheckDistance, groundLayer) &&
                               Physics2D.Raycast(
                               wallCheckBottomPoint.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
            }
            else
            {
                WallDetected = Physics2D.Raycast(
                    wallCheckTopPoint.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
            }
        }

        public void HandleFlip(float xVelocity)
        {
            if (xVelocity > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (xVelocity < 0 && isFacingRight)
            {
                Flip();
            }
        }
        
        public void Flip()
        {
            transform.Rotate(0, 180, 0);
            isFacingRight = !isFacingRight;
            facingDir *= -1;
        }
        
        public void SetVelocity(float xVelocity, float yVelocity)
        {
            if (isKnocked) return;
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
            HandleFlip(xVelocity);
        }

        public void CallCurrentStateTrigger()
        {
            fsm.CurrentState.CallStateTrigger();
        }

#if UNITY_EDITOR
        #region GizmosMethods
        
        protected virtual void OnDrawGizmos/*Selected*/()
        {
            Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + new Vector3(0, -groundCheckDistance));
            Gizmos.DrawLine(wallCheckTopPoint.position, wallCheckTopPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
            Gizmos.color = Color.red;
            if (wallCheckBottomPoint != null)
                Gizmos.DrawLine(wallCheckBottomPoint.position, wallCheckBottomPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
        }
        
        #endregion
#endif
    }
}