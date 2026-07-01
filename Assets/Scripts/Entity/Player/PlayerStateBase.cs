using Core.State;
using UnityEngine;

namespace Entity.Player
{
    public abstract class PlayerStateBase : StateBase<Player>
    {
        protected float stateTimer;
        protected Player player => owner;
        
        protected PlayerStateBase(Player owner, StateMachine stateMachine, string animBoolName)
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            stateTimer -= Time.deltaTime;
        
            owner.ani.SetFloat("yVelocity", owner.rb.linearVelocity.y);

            if (player.InputSet.Player.Dash.WasPressedThisFrame() && CanDash())
            {
                stateMachine.ChangeState(PlayerStateType.Dash);
            }
        }
        
        private bool CanDash()
        {
            if (owner.WallDetected)
                return false;

            if (Equals(stateMachine.CurrentStateKey, PlayerStateType.Dash))
                return false;
        
            return true;
        }
    }
}