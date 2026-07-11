using Core.State;

namespace Entity.Enemy.State
{
    public class AttackState : EnemyStateBase
    {
        public AttackState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
        }

        // public override void OnEnter()
        // {
        //     base.OnEnter();
        //     enemy.SetVelocity(0, rb.linearVelocity.y);
        // }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // string debugLog = stateTrigger ? "stateTrigger is true" : "stateTrigger is false";
            // Debug.Log(debugLog);
            if (stateTrigger)
            {
                stateMachine.ChangeState(EnemyStateType.Battle);
            }
        }
    }
}