public class DashState : EntityState
{
    private float _originalGravityScale;
    
    public DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        _originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(player.facingDir * player.dashSpeed, 0);

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