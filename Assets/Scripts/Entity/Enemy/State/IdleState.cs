using Core.State;

namespace Entity.Enemy.State
{
    public class IdleState : EnemyStateBase
    {
        public IdleState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            stateTimer = enemy.idleDuration;
            enemy.SetVelocity(0, rb.linearVelocity.y);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (stateTimer < 0)
            {
                stateMachine.ChangeState(EnemyStateType.Move);
            }
        }
    }
}