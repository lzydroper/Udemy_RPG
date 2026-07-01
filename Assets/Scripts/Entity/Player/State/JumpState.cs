using Core.State;

namespace Entity.Player.State
{
    public class JumpState : AirState
    {
        public JumpState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            player.SetVelocity(player.rb.linearVelocity.x, player.jumpForce);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.rb.linearVelocity.y < 0 && stateMachine.CurrentStateKey != PlayerStateType.JumpAttack)
            {
                stateMachine.ChangeState(PlayerStateType.Fall);
            }

            if (player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }
    }
}