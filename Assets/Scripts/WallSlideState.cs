public class WallSlideState : EntityState
{
    public WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();

        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.WallJumpState);
        }

        if (!player.WallDetected)
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (player.GroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    private void HandleWallSlide()
    {
        if (player.MoveInput.y < 0)
        {
            player.SetVelocity(player.MoveInput.x, rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.MoveInput.x, player.MoveInput.y * player.wallSlideSlowMultiplier);
        }
    }
}