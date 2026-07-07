using UnityEngine;

namespace Core.State
{
    public abstract class StateBase : IState 
    {
        protected Animator ani;
        protected Rigidbody2D rb;
        protected readonly StateMachine stateMachine;
        protected readonly string animBoolName;
        protected float stateTimer;
        protected bool stateTrigger;

        protected StateBase(StateMachine stateMachine, string animBoolName)
        {
            this.stateMachine = stateMachine;
            this.animBoolName = animBoolName;
        }

        public virtual void OnEnter()
        {
            ani.SetBool(animBoolName, true);
            stateTrigger = false;
        }

        public virtual void OnUpdate()
        {
            stateTimer -= Time.deltaTime;
            UpdateAnimationParas();
        }

        public virtual void OnExit()
        {
            ani.SetBool(animBoolName, false);
        }
        
        public virtual void CallStateTrigger()
        {
            // Debug.Log("CallStateTrigger!");
            stateTrigger = true;
        }
        
        protected virtual void UpdateAnimationParas()
        {
        }
    }
}