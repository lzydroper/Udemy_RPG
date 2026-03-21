public class IdleState : GroundedState
{
    public IdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x != 0/* && !player.WallDetected*/)
        {
            stateMachine.ChangeState(player.MoveState);
        }
    }
}