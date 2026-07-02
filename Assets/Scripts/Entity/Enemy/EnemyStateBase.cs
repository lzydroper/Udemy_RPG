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

        public override void OnEnter()
        {
            base.OnEnter();
            
            enemy.ani.SetFloat("aniMoveMultiplier", enemy.aniMoveMultiplier);
        }
    }
}