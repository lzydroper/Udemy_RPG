using Core.State;

namespace Entity.Player.State
{
    public class DeathState : PlayerStateBase
    {
        public DeathState(Player player, StateMachine stateMachine, string animBoolName) 
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            player.InputSet.Disable();
            rb.simulated = false;
        }
    }
}