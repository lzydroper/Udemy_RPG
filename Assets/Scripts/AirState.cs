public class AirState : EntityState
{
    public AirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.MoveInput.x != 0)
        {
            player.SetVelocity(player.MoveInput.x * (player.moveSpeed * player.inAirMultiplier), rb.linearVelocity.y);
        }

        if (input.Player.BasicAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.JumpAttackState);
        }
    }
}