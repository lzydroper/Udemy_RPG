public class FallState : AirState
{
    public FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
        // player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (player.GroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.WallDetected)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }
}