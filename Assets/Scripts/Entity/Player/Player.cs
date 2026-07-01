using System.Collections;
using Core.State;
using Entity.Player.State;
using UnityEngine;

namespace Entity.Player
{
    public class Player : EntityBase
    {
        // input system
        public Vector2 MoveInput { get; private set; } 
        public PlayerInputSet InputSet { get; private set; }
        // state machine
        [SerializeField] private StateMachine<PlayerStateType> fsm;
    
        // attack
        [Header("attack details")] 
        public float basicAttackDuration;
        public float comboAttackTime;
        public Vector2[] basicAttackVelocity;
        public Vector2 jumpAttackVelocity;
        private Coroutine _queuedAttackCo;
    
        // move
        [Header("movement details")]
        public float moveSpeed;
        public float jumpForce;
        public Vector2 wallJumpForce;
        [Range(0,1)]
        public float inAirMultiplier;
        [Range(0,1)]
        public float wallSlideSlowMultiplier;
        public float dashDuration;
        public float dashSpeed;
        
        #region InheritMethods

        protected override void BuildStateMachine()
        {
            fsm = new StateMachine<PlayerStateType>();
            fsm.AddState(PlayerStateType.Idle, new IdleState(this, fsm, "idle"));
            fsm.AddState(PlayerStateType.Move, new MoveState(this, fsm, "move"));
            fsm.AddState(PlayerStateType.Jump, new JumpState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.Fall, new FallState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.WallSlide, new WallSlideState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.WallJump, new WallJumpState(this, fsm, "jumpFall"));
            fsm.AddState(PlayerStateType.Dash, new DashState(this, fsm, "dash"));
            fsm.AddState(PlayerStateType.BasicAttack, new BasicAttackState(this, fsm, "basicAttack"));
            fsm.AddState(PlayerStateType.JumpAttack, new JumpAttackState(this, fsm, "jumpAttack"));
        }
        
        #endregion

        #region lifecycle
    
        private void Awake()
        {
            FetchComponents();
            InputSet = new PlayerInputSet();
            BuildStateMachine();
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

        protected override void Update()
        {
            base.Update();
            fsm.OnUpdate();
        }

        #endregion

        public void CallCurrentStateTrigger()
        {
            fsm.CurrentState.CallStateTrigger();
        }
    
        public void EnterAttackStateWithDelay()
        {
            if (_queuedAttackCo != null)
            {
                StopCoroutine(_queuedAttackCo);
            }
            _queuedAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
        }
        private IEnumerator EnterAttackStateWithDelayCo()
        {
            yield return new WaitForEndOfFrame();
            fsm.ChangeState(PlayerStateType.BasicAttack);
        }
    }
}