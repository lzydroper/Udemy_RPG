using Core.State;

namespace Entity.Player.State
{
    public class MoveState : GroundedState
    {
        public MoveState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.rb.linearVelocity.x == 0)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        
            player.SetVelocity(player.moveSpeed * player.MoveInput.x, player.rb.linearVelocity.y);
        }
    }
}