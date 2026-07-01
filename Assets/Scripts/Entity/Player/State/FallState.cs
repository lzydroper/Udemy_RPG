using Core.State;

namespace Entity.Player.State
{
    public class FallState : AirState
    {
        public FallState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            // player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }

            if (player.WallDetected)
            {
                stateMachine.ChangeState(PlayerStateType.WallSlide);
            }
        }
    }
}