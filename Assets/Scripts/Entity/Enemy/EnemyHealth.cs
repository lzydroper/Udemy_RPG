using UnityEngine;

namespace Entity.Enemy
{
    public class EnemyHealth : EntityHealth
    {
        #region Variables

        private EnemyBase _enemyBase;

        #endregion

        #region LifecycleMethods

        protected override void Awake()
        {
            base.Awake();
            _enemyBase = GetComponent<EnemyBase>();
        }

        #endregion

        public override void TakeDamage(float damage, Transform damageDealer)
        {
            if (damageDealer?.GetComponent<Player.Player>())
            {
                _enemyBase.TryEnterBattleState(damageDealer);
            }
            base.TakeDamage(damage, damageDealer);
        }
    }
}