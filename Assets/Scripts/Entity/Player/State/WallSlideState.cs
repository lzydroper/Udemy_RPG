using Core.State;

namespace Entity.Player.State
{
    public class WallSlideState : PlayerStateBase
    {
        public WallSlideState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            HandleWallSlide();

            if (input.Player.Jump.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.WallJump);
            }

            if (!player.WallDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Fall);
            }

            if (player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            
                if(player.facingDir != player.MoveInput.x)
                    player.Flip();
            }
        }

        private void HandleWallSlide()
        {
            if (player.MoveInput.y < 0)
            {
                player.SetVelocity(player.MoveInput.x, player.rb.linearVelocity.y);
            }
            else
            {
                player.SetVelocity(player.MoveInput.x, player.MoveInput.y * player.wallSlideSlowMultiplier);
            }
        }
    }
}