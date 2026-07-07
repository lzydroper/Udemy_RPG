using Core.State;

namespace Entity.Enemy
{
    public abstract class EnemyStateBase : StateBase
    {
        protected readonly EnemyBase enemy;
        
        public EnemyStateBase(EnemyBase enemy, StateMachine stateMachine, string animBoolName) 
            : base(stateMachine, animBoolName)
        {
            this.enemy = enemy;
            this.ani = enemy.ani;
            this.rb = enemy.rb;
        }

        protected override void UpdateAnimationParas()
        {
            base.UpdateAnimationParas();
            float aniBattleMultiplier = enemy.attackMoveSpeed / enemy.moveSpeed;
            enemy.ani.SetFloat("aniBattleMultiplier", aniBattleMultiplier);
            
            enemy.ani.SetFloat("aniMoveMultiplier", enemy.aniMoveMultiplier);
            enemy.ani.SetFloat("xVelocity", rb.linearVelocity.x);
        }
    }
}