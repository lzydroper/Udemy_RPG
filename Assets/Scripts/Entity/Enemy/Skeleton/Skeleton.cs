using Entity.Enemy.State;

namespace Entity.Enemy.Skeleton
{
    public class Skeleton : EnemyBase
    {
        #region Variables
        
        #endregion
        
        #region InheritMethods

        protected override void BuildStateMachine()
        {
            base.BuildStateMachine();
            fsm.AddState(SkeletonStateType.Idle, new IdleState(this, fsm, "idle"));
            fsm.AddState(SkeletonStateType.Move, new MoveState(this, fsm, "move"));
            fsm.AddState(SkeletonStateType.Attack, new AttackState(this, fsm, "attack"));
        }
        
        #endregion

        #region LifecycleMethods

        private void Start()
        {
            fsm.Initialize(SkeletonStateType.Idle);
        }
        
        protected override void Update()
        {
            base.Update();
            fsm.OnUpdate();
        }

        #endregion
    }
}