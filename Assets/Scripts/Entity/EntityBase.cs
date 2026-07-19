using System;
using System.Collections;
using Core.Utils;
using UnityEngine;
using VInspector;
using StateMachine = Core.State.StateMachine;

namespace Entity
{
    public abstract class EntityBase : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("DebugChoice")]
        [SerializeField] private bool ifShowFsmState;
#endif
        
        #region IStateOwner
        
        public Animator ani { get; private set; }
        public Rigidbody2D rb { get; private set; }
        public EntityStat stat { get; private set; }

        #endregion

        #region Variables

        public event Action OnFlip;
        
        protected StateMachine fsm;

        // move
        protected bool isFacingRight = true;
        public float facingDir { get; private set; } = 1;
        // collision detect
        [EndFoldout]
        [Header("collision detection")]
        [SerializeField] protected LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 1;
        [SerializeField] private float wallCheckDistance = 1;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private Transform wallCheckTopPoint;
        [SerializeField] private Transform wallCheckBottomPoint;
        public bool GroundDetected { get; private set; }
        public bool WallDetected { get; private set; }
        private bool needSecondaryWallCheck;

        private bool isKnocked;
        private Coroutine knockbackCoroutine;

        private Coroutine slowDownCoroutine;

        #endregion
        
        #region InheritMethods
        
        protected virtual void FetchComponents()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody2D>();
            stat = GetComponent<EntityStat>();
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
#if UNITY_EDITOR
            if (ifShowFsmState && fsm is not null)
            {
                Logwin.Log("PreStateKey", fsm.PreStateKey, gameObject.name);
                Logwin.Log("CurStateKey", fsm.CurStateKey, gameObject.name);
            }
#endif
        }

        #endregion
        
        public void SlowDown(float duration, float slowMultiplier) 
            => this.RestartCoroutine(ref slowDownCoroutine, SlowDownCo(duration, slowMultiplier));

        protected virtual IEnumerator SlowDownCo(float duration, float slowMultiplier)
        {
            yield return null;
        }

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
            OnFlip?.Invoke();
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