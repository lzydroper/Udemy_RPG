using UnityEngine;

namespace Entity.Enemy
{
    public class EnemyAnimationTriggers : EntityAnimationTriggers
    {
        #region Variables

        private EnemyBase _enemyBase;
        private EnemyVFX _enemyVFX;

        #endregion

        #region LifecycleMethods

        protected override void Awake()
        {
            base.Awake();
            _enemyBase = GetComponentInParent<EnemyBase>();
            _enemyVFX = GetComponentInParent<EnemyVFX>();
        }

        #endregion

        private void EnableCounterWindow()
        {
            _enemyBase.SwitchStunEnable(true);
            _enemyVFX.SwitchAlertEnable(true);
        }

        private void DisableCounterWindow()
        {
            _enemyBase.SwitchStunEnable(false);
            _enemyVFX.SwitchAlertEnable(false);
        }
    }
}