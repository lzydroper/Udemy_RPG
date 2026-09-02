using Data;

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

        public override bool TryTakeDamage(in DamageContext context)
        {
            if (base.TryTakeDamage(context))
            {
                if (!isDead && context.DamageDealer.TryGetComponent<Player.Player>(out _))
                {
                    _enemyBase.TryEnterBattleState(context.DamageDealer);
                }
                return true;
            }
            return false;
        }
    }
}