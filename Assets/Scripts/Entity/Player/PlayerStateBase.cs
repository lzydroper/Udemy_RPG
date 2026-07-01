using System;
using Core.State;
using UnityEngine;

namespace Entity.Player
{
    public abstract class PlayerStateBase : StateBase<Player, PlayerStateType>
    {
        protected PlayerInputSet input;
        protected float stateTimer;
        protected bool stateTrigger;
        protected Player player => owner;
        
        protected PlayerStateBase(Player owner, StateMachine<PlayerStateType> stateMachine, string animBoolName)
            : base(owner, stateMachine, animBoolName)
        {
            this.input = owner.InputSet;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stateTrigger = false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            stateTimer -= Time.deltaTime;
        
            owner.ani.SetFloat("yVelocity", owner.rb.linearVelocity.y);

            if (input.Player.Dash.WasPressedThisFrame() && CanDash())
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

        public void CallStateTrigger()
        {
            stateTrigger = true;
        }
    }
}