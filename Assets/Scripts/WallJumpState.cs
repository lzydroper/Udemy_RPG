public class WallJumpState : EntityState
{
    public WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocity(player.wallJumpForce.x * - player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (player.WallDetected)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }
}