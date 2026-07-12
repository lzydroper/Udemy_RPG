using Entity.Interfaces;
using UnityEngine;

namespace Entity.Player
{
    public class PlayerCombat : EntityCombat
    {
        #region Variables

        [Header("CounterAttack")] 
        [SerializeField] private float counterDuration = 0.25f;
        public float CounterDuration => counterDuration;

        #endregion

        #region LifecycleMethods

        

        #endregion

        // ReSharper disable Unity.PerformanceAnalysis
        public bool TryCounterAttack()
        {
            bool success = false;
            foreach (var target in GetDetectTargets())
            {
                var counterable = target.GetComponent<ICounterable>();
                if (counterable == null)
                    continue;
                if (counterable.CanBeCountered)
                {
                    success = true;
                    counterable.HandleCounter();
                }
            }
            return success;
        }
    }
}