using System;

namespace Core.State
{
    public abstract class StateBase<TOwner> : IState 
        where TOwner : IStateOwner
    {
        protected readonly TOwner owner;
        protected readonly StateMachine stateMachine;
        protected readonly string animBoolName;
        protected bool stateTrigger;

        protected StateBase(TOwner owner, StateMachine stateMachine, string animBoolName)
        {
            this.owner = owner;
            this.stateMachine = stateMachine;
            this.animBoolName = animBoolName;
        }

        public virtual void OnEnter()
        {
            owner.ani.SetBool(animBoolName, true);
            stateTrigger = false;
        }
        
        public virtual void OnUpdate() { }

        public virtual void OnExit()
        {
            owner.ani.SetBool(animBoolName, false);
        }
        
        public virtual void CallStateTrigger()
        {
            stateTrigger = true;
        }
    }
}