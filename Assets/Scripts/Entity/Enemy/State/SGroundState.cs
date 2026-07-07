using Core.State;

namespace Entity.Enemy.State
{
    public abstract class SGroundState : EnemyStateBase
    {
        public SGroundState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (enemy.PlayerDetected())
            {
                stateMachine.ChangeState(EnemyStateType.Battle);
            }
        }
    }
}