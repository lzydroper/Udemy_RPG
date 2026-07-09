using Core.State;
using UnityEngine;

namespace Entity.Enemy.State
{
    public class StunnedState : EnemyStateBase
    {
        private EnemyVFX _enemyVFX;
        
        public StunnedState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
            _enemyVFX = enemy.GetComponent<EnemyVFX>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyVFX.SwitchAlertEnable(false);
            enemy.SwitchStunEnable(false);
            stateTimer = enemy.stunDuration;
            rb.linearVelocity = new Vector2(enemy.stunVelocity.x * enemy.facingDir * -1, enemy.stunVelocity.y);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(EnemyStateType.Idle);
            }
        }
    }
}