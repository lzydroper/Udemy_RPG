using Core.State;

namespace Entity.Player.State
{
    public abstract class AirState : PlayerStateBase
    {
        protected AirState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.MoveInput.x != 0)
            {
                player.SetVelocity(player.MoveInput.x * (player.moveSpeed * player.inAirMultiplier), rb.linearVelocity.y);
            }

            if (player.InputSet.Player.BasicAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.JumpAttack);
            }
        }
    }
}