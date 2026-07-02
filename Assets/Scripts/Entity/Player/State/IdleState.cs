using Core.State;

namespace Entity.Player.State
{
    public class IdleState : GroundedState
    {
        public IdleState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        
            player.SetVelocity(0, rb.linearVelocity.y);
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