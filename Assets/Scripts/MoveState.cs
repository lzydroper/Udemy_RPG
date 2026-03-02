public class MoveState : GroundedState
{
    public MoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.x == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
        player.SetVelocity(player.moveSpeed * player.MoveInput.x, rb.linearVelocity.y);
    }
}