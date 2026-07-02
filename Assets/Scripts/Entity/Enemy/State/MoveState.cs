using Core.State;

namespace Entity.Enemy.State
{
    public class MoveState : SGroundState
    {
        public MoveState(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(enemy, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (!enemy.GroundDetected || enemy.WallDetected)
            {
                enemy.Flip();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!enemy.GroundDetected || enemy.WallDetected)
            {
                stateMachine.ChangeState(EnemyStateType.Idle);
            }
            
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
        }
    }
}