using Core.State;

namespace Entity.Player.State
{
    public abstract class GroundedState : PlayerStateBase
    {
        protected GroundedState(Player owner, StateMachine stateMachine, string animBoolName )
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

            if (player.InputSet.Player.Jump.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.Jump);
            }

            if (player.InputSet.Player.BasicAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.BasicAttack);
            }
        }
    }
}