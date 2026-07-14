using System.Collections;
using Stats;
using UnityEngine;
using VInspector;

namespace Entity
{
    public class EntityStatusHandler : MonoBehaviour
    {
#if UNITY_EDITOR
        [Foldout("DebugChoice")]
        [SerializeField] private bool ifShowCurElementType;
        [EndFoldout]
#endif
        
        #region Variables

        private EntityBase _entityBase;
        private EntityStat _entityStat;
        private EntityHealth _entityHealth;
        private EntityVFX _entityVFX;
        private ElementType _curElementType = ElementType.None;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            _entityBase = GetComponent<EntityBase>();
            _entityHealth = GetComponent<EntityHealth>();
            _entityStat = GetComponent<EntityStat>();
            _entityVFX = GetComponent<EntityVFX>();
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (ifShowCurElementType)
                Logwin.Log("CurElementType", _curElementType.ToString(), gameObject.name);
        }
#endif

        #endregion

        public bool CanBeAppliedElementEffect(ElementType type)
        {
            return _curElementType == ElementType.None;
        }

        public void ApplyFireEffect(float duration, float totalDamage)
        {
            var multiplier = 1 - _entityStat.GetElementResistancePer(ElementType.Fire);
            totalDamage *= multiplier;
            StartCoroutine(FireEffectCo(duration, totalDamage));
        }

        private IEnumerator FireEffectCo(float duration, float totalDamage)
        {
            // set type
            _curElementType = ElementType.Fire;
            // play vfx
            _entityVFX?.PlayOnStatusVFX(duration, ElementType.Fire);
            // apply specific effect
            const int freqPerSec = 2;
            const float interval = 1f / freqPerSec;
            float counts = Mathf.RoundToInt(freqPerSec * duration);
            float damage = totalDamage / counts;
            for (int i = 0; i < counts; i++)
            {
                _entityHealth.ReduceHealth(damage);
                yield return new WaitForSeconds(interval);
            }
            // reset type
            _curElementType = ElementType.None;
        }

        public void ApplyIceEffect(float duration, float slowMultiplier)
        {
            var multiplier =  1 - _entityStat.GetElementResistancePer(ElementType.Ice);
            duration *= multiplier;
            StartCoroutine(IceEffectCo(duration, slowMultiplier));
        }

        private IEnumerator IceEffectCo(float duration, float slowMultiplier)
        {
            // set type
            _curElementType = ElementType.Ice;
            // play vfx
            _entityVFX?.PlayOnStatusVFX(duration, ElementType.Ice);
            // apply specific effect
            _entityBase.SlowDown(duration, slowMultiplier);
            yield return new WaitForSeconds(duration);
            // reset type
            _curElementType = ElementType.None;
        }
    }
}