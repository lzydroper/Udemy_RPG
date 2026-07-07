using Core.State;
using Entity.Enemy.Skeleton;
using UnityEngine;

namespace Entity.Enemy.State
{
    public class BattleState : EnemyStateBase
    {
        private Transform player;
        private bool _hadGetPlayer;
        private float _lastTimeWasInBattle;
        
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
                player = enemy.PlayerDetected().transform;
            }
            // check if should retreat
            if (DistanceToPlayer() < enemy.retreatDistance)
            {
                rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), rb.linearVelocity.y);
                enemy.HandleFlip(DirectionToPlayer());
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (enemy.PlayerDetected())
            {
                // update battle timer
                _lastTimeWasInBattle = Time.time;
            }
            // check battle is over
            if (enemy.battleDuration < Time.time - _lastTimeWasInBattle)
            {
                stateMachine.ChangeState(SkeletonStateType.Idle);
            }

            if (WithInAttackRange() && enemy.PlayerDetected())
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