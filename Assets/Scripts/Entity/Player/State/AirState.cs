using Core.State;

namespace Entity.Player.State
{
    public abstract class AirState : PlayerStateBase
    {
        protected AirState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.MoveInput.x != 0)
            {
                player.SetVelocity(player.MoveInput.x * (player.moveSpeed * player.inAirMultiplier), player.rb.linearVelocity.y);
            }

            if (input.Player.BasicAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.JumpAttack);
            }
        }
    }
}