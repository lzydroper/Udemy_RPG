public class GroundedState : EntityState
{
    public GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (rb.linearVelocity.y < 0 && !player.GroundDetected)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpState);
        }

        if (input.Player.BasicAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.BasicAttackState);
        }
    }
}