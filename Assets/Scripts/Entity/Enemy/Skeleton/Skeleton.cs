using Core.State;
using UnityEngine;

namespace Entity.Enemy.Skeleton
{
    public class Skeleton : EnemyBase
    {
        #region Variables

        // state machine
        [SerializeField] private StateMachine<SkeletonStateType> fsm;

        #endregion
        
        #region InheritMethods

        protected override void BuildStateMachine()
        {
            
        }
        
        #endregion

        #region Lifecycle

        // private void Start()
        // {
        //     
        // }
        //
        // protected override void Update()
        // {
        //     base.Update();
        //     fsm.OnUpdate();
        // }

        #endregion
    }
}