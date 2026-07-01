using System;
using Core.State;

namespace Entity.Enemy
{
    public class EnemyStateBase<TOwner, TStateType> : StateBase<TOwner, TStateType>
        where TOwner : EnemyBase
        where TStateType : Enum
    {
        public EnemyStateBase(TOwner owner, StateMachine<TStateType> stateMachine, string animBoolName) 
            : base(owner, stateMachine, animBoolName)
        {
        }
    }
}