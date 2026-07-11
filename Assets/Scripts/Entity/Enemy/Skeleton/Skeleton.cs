using Entity.Enemy.State;
using Entity.Interfaces;
using VInspector;

namespace Entity.Enemy.Skeleton
{
    public class Skeleton : EnemyBase, ICounterable
    {
        #region Variables

        public bool CanBeCountered => canBeStunned;
        
        #endregion
        
        #region InheritMethods

        protected override void BuildStateMachine()
        {
            base.BuildStateMachine();
            fsm.AddState(SkeletonStateType.Idle, new IdleState(this, fsm, "idle"));
            fsm.AddState(SkeletonStateType.Move, new MoveState(this, fsm, "move"));
            fsm.AddState(SkeletonStateType.Attack, new AttackState(this, fsm, "attack"));
            fsm.AddState(SkeletonStateType.Battle, new BattleState(this, fsm, "battle"));
            fsm.AddState(SkeletonStateType.Death, new DeathState(this, fsm));
            fsm.AddState(SkeletonStateType.Stunned, new StunnedState(this, fsm, "stunned"));
        }
        
        #endregion

        #region LifecycleMethods

        private void Start()
        {
            fsm.Initialize(SkeletonStateType.Idle);
        }

        #endregion

        [Button("TestStunned")]
        public void HandleCounter()
        {
            if (!CanBeCountered) return;
            fsm.ChangeState(SkeletonStateType.Stunned);
        }
    }
}