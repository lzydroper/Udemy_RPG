using System.Collections;
using System.Linq;
using Core.Utils;
using Entity.Player.State;
using UnityEngine;

namespace Entity.Player
{
    public class Player : EntityBase
    {
        // input system
        public Vector2 MoveInput { get; private set; } 
        public PlayerInputSet InputSet { get; private set; }
    
        // attack
        [Header("attack details")] 
        public float basicAttackDuration = 0.25f;
        public float comboAttackTime = 1f;
        public Vector2[] basicAttackVelocity;
        public Vector2 jumpAttackVelocity = new(3, -6);
    
        // move
        [Header("movement details")]
        public float moveSpeed = 8;
        public float jumpForce = 15;
        public Vector2 wallJumpForce = new(8, 15);
        [Range(0,1)]
        public float inAirMultiplier = 0.65f;
        [Range(0,1)]
        public float wallSlideSlowMultiplier = 0.35f;
        public float dashDuration = 0.25f;
        public float dashSpeed = 20f;
        
        #region InheritMethods

        protected override void BuildStateMachine()
        {
            base.BuildStateMachine();
            fsm.AddState(PlayerStateType.Idle, new IdleState(this, fsm, "idle"));
            fsm.AddState(PlayerStateType.Move, new MoveState(this, fsm, "move"));
            fsm.AddState(PlayerStateType.Jump, new JumpState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.Fall, new FallState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.WallSlide, new WallSlideState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.WallJump, new WallJumpState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.Dash, new DashState(this, fsm, "dash"));
            fsm.AddState(PlayerStateType.BasicAttack, new BasicAttackState(this, fsm, "basicAttack"));
            fsm.AddState(PlayerStateType.JumpAttack, new JumpAttackState(this, fsm, "jumpAttack"));
            fsm.AddState(PlayerStateType.Death, new DeathState(this, fsm, "death"));
            fsm.AddState(PlayerStateType.CounterAttack, new CounterAttackState(this, fsm, "counterAttack"));
        }

        public override void Death()
        {
            base.Death();
            fsm.ChangeState(PlayerStateType.Death);
        }

        #endregion

        #region LifecycleMethods
    
        protected override void Awake()
        {
            base.Awake();
            InputSet = new PlayerInputSet();
        }

        private void OnEnable()
        {
            InputSet.Enable();
            InputSet.Player.Movement.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            InputSet.Player.Movement.canceled += ctx => MoveInput = Vector2.zero;
        }

        private void OnDisable()
        {
            InputSet.Disable();
        }

        private void Start()
        {
            fsm.Initialize(PlayerStateType.Idle);
        }

        #endregion

        protected override IEnumerator SlowDownCo(float duration, float slowMultiplier)
        {
            float oriMoveSpeed = moveSpeed;
            float oriJumpForce = jumpForce;
            float oriAniSpeed = ani.speed;
            Vector2 oriWallJumpForce = wallJumpForce;
            Vector2 oriJumpAttackVelocity = jumpAttackVelocity;
            Vector2[] oriBasicAttackVelocities = basicAttackVelocity.ToArray();
            
            moveSpeed *= slowMultiplier;
            jumpForce *= slowMultiplier;
            ani.speed *= slowMultiplier;
            wallJumpForce *= slowMultiplier;
            jumpAttackVelocity *= slowMultiplier;
            for (int i = 0; i < basicAttackVelocity.Length; i++)
                basicAttackVelocity[i] *= slowMultiplier;
            
            yield return new WaitForSeconds(duration);
            
            moveSpeed = oriMoveSpeed;
            jumpForce = oriJumpForce;
            ani.speed = oriAniSpeed;
            wallJumpForce = oriWallJumpForce;
            jumpAttackVelocity = oriJumpAttackVelocity;
            for (int i = 0; i < basicAttackVelocity.Length; i++)
                basicAttackVelocity[i] = oriBasicAttackVelocities[i];
        }
    }
}