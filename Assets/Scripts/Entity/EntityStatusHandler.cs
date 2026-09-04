using System.Collections;
using Core.Utils;
using Data;
using Generated;
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
        [SerializeField] private bool ifShowCurCharge;
        [EndFoldout]
#endif
        
        #region Variables

        private EntityBase _entityBase;
        private EntityStat _entityStat;
        private EntityHealth _entityHealth;
        private EntityVFX _entityVFX;
        private ElementType _curElementType = ElementType.None;
        [Header("Thunder Effect Details")] 
        private float _curCharge;
        private const float ChargeThreshold = 1f;
        private Coroutine thunderCoroutine;

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
            if (ifShowCurCharge)
                Logwin.Log("CurCharge", _curCharge, gameObject.name);
        }
#endif

        #endregion

        public void ApplyElementEffect(ElementType elementType, ElementalEffectData effectData)
        {
            if (elementType == ElementType.Ice && CanBeAppliedElementEffect(ElementType.Ice))
                ApplyIceEffect(effectData.iceDuration, effectData.iceSlowMultiplier);
            if (elementType == ElementType.Fire && CanBeAppliedElementEffect(ElementType.Fire))
                ApplyFireEffect(effectData.fireDuration, effectData.fireTotalDamage);
            if (elementType == ElementType.Thunder && CanBeAppliedElementEffect(ElementType.Thunder))
                ApplyThunderEffect(effectData.thunderDuration, effectData.thunderDamage, effectData.thunderCharge);
        }
        
        private bool CanBeAppliedElementEffect(ElementType type)
        {
            // 电属性效果可以被自身刷新
            if (type == ElementType.Thunder && _curElementType == ElementType.Thunder)
                return true;
            return _curElementType == ElementType.None;
        }
        
        /*
         * 电击效果流程：
         * 打击时
         * 1.如果处于电流，增加蓄能，刷新电流
         * 2.如果不处于电流，开始电流，触发蓄能
         * 3.电流：一定时间内播放电流特效，然后清除蓄能
         * 4.蓄能：达到阈值时，触发雷劈
         * 5.雷劈：清除蓄能，播放一次雷劈特效，造成一次雷劈伤害
         * 6.雷劈和电流独立
         */

        private void ApplyThunderEffect(float duration, float damage, float charge)
        {
            var multiplier = 1 - _entityStat.GetElementResistancePer(ElementType.Thunder);
            _curCharge += charge * multiplier;
            if (_curCharge > ChargeThreshold)
            {
                DoThunderStrike(damage);
                StopThunderEffect();
                //TODO:感觉不应该Return，因为触发雷劈的那次攻击，也应该继续刷新电流
                return;
            }
            this.RestartCoroutine(ref thunderCoroutine, ThunderEffectCo(duration));
        }

        private IEnumerator ThunderEffectCo(float duration)
        {
            // set type
            _curElementType = ElementType.Thunder;
            // play vfx
            _entityVFX?.PlayOnStatusVFX(duration, ElementType.Thunder);
            // apply specific effect
            yield return new WaitForSeconds(duration);
            // reset type
            StopThunderEffect();
        }

        private void DoThunderStrike(float damage)
        {
            GameManager.Pool.RentAsync(Keys.Pool.OnThunderHitVFX, 
                position: transform.position, parent: transform);
            _entityHealth.ReduceHealth(damage);
        }

        private void StopThunderEffect()
        {
            _curElementType = ElementType.None;
            _entityVFX?.ResetAllVFX();
            _curCharge = 0;
        }
        
        private void ApplyFireEffect(float duration, float totalDamage)
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

        private void ApplyIceEffect(float duration, float slowMultiplier)
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