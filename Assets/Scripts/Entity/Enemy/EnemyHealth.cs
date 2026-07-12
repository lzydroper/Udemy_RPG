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

        public override bool TryTakeDamage(float damage, Transform damageDealer)
        {
            if (base.TryTakeDamage(damage, damageDealer))
            {
                if (!isDead && damageDealer.TryGetComponent<Player.Player>(out _))
                {
                    _enemyBase.TryEnterBattleState(damageDealer);
                }
                return true;
            }
            return false;
        }
    }
}