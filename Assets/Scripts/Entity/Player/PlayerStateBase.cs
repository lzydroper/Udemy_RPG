using Core.State;

namespace Entity.Player
{
    public abstract class PlayerStateBase : StateBase
    {
        protected readonly Player player;
        
        protected PlayerStateBase(Player player, StateMachine stateMachine, string animBoolName)
            : base(stateMachine, animBoolName)
        {
            this.player = player;
            this.ani = player.ani;
            this.rb = player.rb;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (player.InputSet.Player.Dash.WasPressedThisFrame() && CanDash())
            {
                stateMachine.ChangeState(PlayerStateType.Dash);
            }
        }

        protected override void UpdateAnimationParas()
        {
            base.UpdateAnimationParas();
            ani.SetFloat("yVelocity", rb.linearVelocity.y);
        }

        private bool CanDash()
        {
            if (player.WallDetected)
                return false;

            if (stateMachine.CurStateKey == PlayerStateType.Dash)
                return false;
        
            return true;
        }
    }
}