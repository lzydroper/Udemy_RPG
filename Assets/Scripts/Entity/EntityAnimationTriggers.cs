using UnityEngine;

namespace Entity
{
    public class EntityAnimationTriggers : MonoBehaviour
    {
        private EntityBase _entityBase;
        private EntityCombat _entityCombat;

        protected virtual void Awake()
        {
            _entityBase = GetComponentInParent<EntityBase>();
            _entityCombat = GetComponentInParent<EntityCombat>();
        }

        private void CurrentStateTrigger()
        {
            _entityBase.CallCurrentStateTrigger();
        }

        private void AttackTrigger()
        {
            _entityCombat.PerformAttack();
        }
    }
}