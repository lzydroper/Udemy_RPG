using Core.State;

namespace Entity.Player.State
{
    public class JumpState : AirState
    {
        public JumpState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (rb.linearVelocity.y < 0 && stateMachine.CurStateKey != PlayerStateType.JumpAttack)
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