using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    // input system
    public Vector2 MoveInput { get; private set; } 
    public PlayerInputSet InputSet { get; private set; }
    // components
    public Animator Anim { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    // state machine
    private StateMachine _stateMachine;
    public EntityState IdleState { get; private set; }
    public EntityState MoveState { get; private set; }
    public EntityState JumpState { get; private set; }
    public EntityState FallState { get; private set; }
    public EntityState WallSlideState { get; private set; }
    public EntityState WallJumpState { get; private set; }
    public EntityState DashState { get; private set; }
    public EntityState BasicAttackState { get; private set; }
    public EntityState JumpAttackState { get; private set; }
    
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
    private bool _isFacingRight = true;
    public float facingDir { get; private set; } = 1;
    [Range(0,1)]
    public float inAirMultiplier;
    [Range(0,1)]
    public float wallSlideSlowMultiplier;
    public float dashDuration;
    public float dashSpeed;
    
    // collision detect
    [Header("collision detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallCheckTopPoint;
    [SerializeField] private Transform wallCheckBottomPoint;
    public bool GroundDetected { get; private set; }
    public bool WallDetected { get; private set; }

    #region lifecycle
    
    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        
        InputSet = new PlayerInputSet();
        
        _stateMachine = new StateMachine();
        IdleState = new IdleState(this, _stateMachine, "idle");
        MoveState = new MoveState(this, _stateMachine, "move");
        JumpState = new JumpState(this, _stateMachine, "jumpFall");
        FallState = new FallState(this, _stateMachine, "jumpFall");
        WallSlideState = new WallSlideState(this, _stateMachine, "wallSlide");
        WallJumpState = new WallJumpState(this, _stateMachine, "jumpFall");
        DashState = new DashState(this, _stateMachine, "dash");
        BasicAttackState = new BasicAttackState(this, _stateMachine, "basicAttack");
        JumpAttackState = new JumpAttackState(this, _stateMachine, "jumpAttack");
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
        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        HandleCollisionDetect();
        
        _stateMachine.UpdateActiveState();
    }

    #endregion
    
    #region handleMethods

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
        if (xVelocity > 0 && !_isFacingRight)
        {
            Flip();
        }
        else if (xVelocity < 0 && _isFacingRight)
        {
            Flip();
        }
    }
    
    #endregion
    
    #region privateMethods
    
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _isFacingRight = !_isFacingRight;
        facingDir *= -1;
    }
    
    #endregion
    
    #region publicMethods

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void CallCurrentStateTrigger()
    {
        _stateMachine.CallActiveStateTrigger();
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
        _stateMachine.ChangeState(BasicAttackState);
    }
    
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheckTopPoint.position, wallCheckTopPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheckBottomPoint.position, wallCheckBottomPoint.position + new Vector3(facingDir * wallCheckDistance, 0));
    }
}