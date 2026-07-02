using UnityEngine;

namespace Entity.Enemy
{
    public abstract class EnemyBase : EntityBase
    {
        #region Variables

        [Header("Movement")] 
        public float idleDuration;
        public float moveSpeed;
        private float baseMoveSpeed = 1f;
        public float aniMoveMultiplier => moveSpeed / baseMoveSpeed;

        #endregion

        #region LifecycleMethods
        
        protected override void Awake()
        {
            base.Awake();
            baseMoveSpeed = moveSpeed != 0 ? moveSpeed : 1f;
        }
        
        #endregion
    }
}