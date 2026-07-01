using Core.State;

namespace Entity.Player.State
{
    public abstract class GroundedState : PlayerStateBase
    {
        protected GroundedState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            if (player.rb.linearVelocity.y < 0 && !player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Fall);
            }

            if (input.Player.Jump.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.Jump);
            }

            if (input.Player.BasicAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.BasicAttack);
            }
        }
    }
}