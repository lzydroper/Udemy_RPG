using System;
using Core.State;

namespace Entity.Enemy
{
    public class EnemyStateBase<TOwner> : StateBase<TOwner>
        where TOwner : EnemyBase
    {
        public EnemyStateBase(TOwner owner, StateMachine stateMachine, string animBoolName) 
            : base(owner, stateMachine, animBoolName)
        {
        }
    }
}