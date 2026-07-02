using Core.State;

namespace Entity.Player.State
{
    public class MoveState : GroundedState
    {
        public MoveState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (rb.linearVelocity.x == 0)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        
            player.SetVelocity(player.moveSpeed * player.MoveInput.x, rb.linearVelocity.y);
        }
    }
}