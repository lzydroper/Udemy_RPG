using System;
using Core.State;
using UnityEngine;

namespace Entity
{
    public abstract class EntityBase : MonoBehaviour, IStateOwner
    {
        #region IStateOwner
        
        public Animator ani { get; private set; }
        public Rigidbody2D rb { get; private set; }

        #endregion

        #region Variables

        // move
        protected bool isFacingRight = true;
        public float facingDir { get; private set; } = 1;
        // collision detect
        [Header("collision detection")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] private Transform wallCheckTopPoint;
        [SerializeField] private Transform wallCheckBottomPoint;
        public bool GroundDetected { get; private set; }
        public bool WallDetected { get; private set; }

        #endregion
        
        #region InheritMethods
        
        protected virtual void FetchComponents()
        {
            ani = GetComponentInChildren<Animator>();
            rb = GetComponentInChildren<Rigidbody2D>();
        }

        protected abstract void BuildStateMachine();
        
        #endregion

        #region LifecycleMethods

        protected virtual void Update()
        {
            HandleCollisionDetect();
        }

        #endregion
        
        private void HandleCollisionDetect()
        {
            GroundDetected = Physics2D.Raycast(
                transform.position, Vector2.down, groundCheckDistance, groundLayer);
            WallDetected = Physics2D.Raycast(
                               wallCheckTopPoint.position, Vector2.right * facingDir, wallCheckDistance, groundLayer) &&
                           Physics2D.Raycast(
                               wallCheckBottomPoint.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
        }

        private void HandleFlip(float xVelocity)
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
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
            HandleFlip(xVelocity);
        }

#if UNITY_EDITOR
        #region GizmosMethods
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
            Gizmos.DrawLine(wallCheckTopPoint.position, wallCheckTopPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheckBottomPoint.position, wallCheckBottomPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
        }
        
        #endregion
#endif
    }
}