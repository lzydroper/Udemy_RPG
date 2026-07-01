using Core.State;

namespace Entity.Player.State
{
    public class IdleState : GroundedState
    {
        public IdleState(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            player.SetVelocity(0, player.rb.linearVelocity.y);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (player.MoveInput.x != 0/* && !player.WallDetected*/)
            {
                stateMachine.ChangeState(PlayerStateType.Move);
            }
        }
    }
}