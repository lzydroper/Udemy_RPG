using System;
using System.Collections;
using Stats;
using UnityEngine;

namespace Entity
{
    public class EntityStatusHandler : MonoBehaviour
    {
        #region Variables

        private EntityBase _entityBase;
        private EntityStat _entityStat;
        private EntityVFX _entityVFX;
        private ElementType _curElementType = ElementType.None;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            _entityBase = GetComponent<EntityBase>();
            _entityStat = GetComponent<EntityStat>();
            _entityVFX = GetComponent<EntityVFX>();
        }

        #endregion

        public bool CanBeAppliedElementEffect(ElementType type)
        {
            return _curElementType == ElementType.None;
        }

        public void ApplyIceEffect(float duration, float slowMultiplier)
        {
            var multiplier =  1 - _entityStat.GetElementResistancePer(ElementType.Ice);
            duration *= multiplier;
            _entityBase.SlowDown(duration, slowMultiplier);
            StartCoroutine(IceEffectCo(duration));
        }

        private IEnumerator IceEffectCo(float duration)
        {
            _curElementType = ElementType.Ice;
            _entityVFX?.PlayOnStatusVFX(duration, ElementType.Ice);
            yield return new WaitForSeconds(duration);
            _curElementType = ElementType.None;
        }
    }
}