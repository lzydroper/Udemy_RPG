public class DashState : EntityState
{
    private float _originalGravityScale;
    private float _dashDir;
    
    public DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _dashDir = (player.MoveInput.x != 0) ? player.MoveInput.x : player.facingDir;
        _originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        
        // cancel dash logic
        if (player.WallDetected)
        {
            if (player.GroundDetected)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.WallSlideState);
            }
        }
        
        player.SetVelocity(_dashDir * player.dashSpeed, 0);

        if (stateTimer < 0)
        {
            if (player.GroundDetected)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.FallState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.SetVelocity(0, 0);
        rb.gravityScale = _originalGravityScale;
        
    }
}