using Core.State;
using UnityEngine;

namespace Entity
{
    public abstract class EntityStateBase : IState 
    {
        protected Animator ani;
        protected Rigidbody2D rb;
        protected EntityStat stat;
        protected readonly StateMachine stateMachine;
        protected readonly string animBoolName;
        protected float stateTimer;
        protected bool stateTrigger;

        protected EntityStateBase(StateMachine stateMachine, string animBoolName)
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

        protected void UpdateAttackSpeed()
        {
            ani.SetFloat("attackSpeed", stat.attackSpeed.GetValue());
        }
    }
}