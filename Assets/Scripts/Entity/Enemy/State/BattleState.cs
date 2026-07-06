using Core.State;
using UnityEngine;

namespace Entity.Enemy.State
{
    public class BattleState : EnemyStateBase
    {
        private Transform player;
        private bool _hadGetPlayer;
        
        public BattleState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            if (player == null)
            {
                _hadGetPlayer = true;
                player = enemy.PlayerDetection().transform;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (WithInAttackRange())
            {
                stateMachine.ChangeState(EnemyStateType.Attack);
            }
            else
            {
                enemy.SetVelocity(enemy.attackMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);
            }
        }

        private bool WithInAttackRange() => DistanceToPlayer() < enemy.attackDistance;

        private float DirectionToPlayer()
        {
            if (!_hadGetPlayer)
                return 0;
            return player.position.x < enemy.transform.position.x ? -1 : 1;
        }

        private float DistanceToPlayer()
        {
            if (!_hadGetPlayer)
                return float.MaxValue;
            return Mathf.Abs(player.position.x - enemy.transform.position.x);
        }
    }
}