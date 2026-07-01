using Core.State;

namespace Entity.Player.State
{
    public class WallJumpState : PlayerStateBase
    {
        public WallJumpState(Player owner, StateMachine stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            player.SetVelocity(player.wallJumpForce.x * - player.facingDir, player.wallJumpForce.y);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.rb.linearVelocity.y < 0)
            {
                stateMachine.ChangeState(PlayerStateType.Fall);
            }

            if (player.WallDetected)
            {
                stateMachine.ChangeState(PlayerStateType.WallSlide);
            }
        }
    }
}