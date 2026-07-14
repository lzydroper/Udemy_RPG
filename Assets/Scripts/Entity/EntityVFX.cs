using System.Collections;
using Core.Utils;
using Generated;
using Stats;
using UnityEngine;

namespace Entity
{
    public class EntityVFX : MonoBehaviour
    {
        #region Variables

        private SpriteRenderer sr;
        // 受到伤害
        [Header("OnTakingDamageVFX")]
        private Coroutine onDamageCoroutine;
        private Material originalMaterial;
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = .2f;
        // 造成伤害
        [Header("OnDoingDamageVFX")]
        [SerializeField] private Color hitVFXColor = Color.white;
        [Header("ElementColor")]
        private Color originalColor;
        [SerializeField] private Color iceEffectColor = Color.cyan;
        [SerializeField] private Color fireEffectColor = Color.red;
        [SerializeField] private Color lightningColor = Color.yellow;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            originalMaterial = sr.material;
            originalColor = sr.color;
        }

        #endregion

        #region ToolMethods

        private Color _GetColorByType(ElementType type) => type switch
        {
            ElementType.Fire => fireEffectColor,
            ElementType.Ice => iceEffectColor,
            ElementType.Lightning => lightningColor,
            _ => originalColor
        };

        #endregion

        public void PlayOnStatusVFX(float duration, ElementType type)
        {
            if (type == ElementType.None) return;
            StartCoroutine(OnStatusVFXCo(duration, _GetColorByType(type)));
        }

        private IEnumerator OnStatusVFXCo(float duration, Color color)
        {
            Color lightColor = color * 1.2f;
            Color darkColor = color * 0.8f;
            float interval = .25f;
            float timer = 0;
            bool toggle = false;
            while (timer < duration)
            {
                sr.color = toggle ? lightColor : darkColor;
                yield return new WaitForSeconds(interval);
                toggle = !toggle;
                timer += interval;
            }
            sr.color = originalColor;
        }

        public void CreateOnHitVFX(Transform target, bool isCritical)
        {
            string vfxKey = isCritical ? Keys.Pool.OnCritHitVFX : Keys.Pool.OnHitVFX;
            if (GameManager.Pool.TryRent(vfxKey, out var vfx,
                    position: target.position, parent: target))
            {
                vfx.GetComponentInChildren<SpriteRenderer>().color = hitVFXColor;
                vfx.ReturnAsync(Keys.Pool.OnHitVFX);
            }
        }

        public void UpdateOnHitColor(ElementType type)
        {
            hitVFXColor = _GetColorByType(type);
        }

        public void PlayOnDamageVFX()
        {
            if (onDamageCoroutine != null)
                StopCoroutine(onDamageCoroutine);
            StartCoroutine(OnDamageVFXCo());
        }

        private IEnumerator OnDamageVFXCo()
        {
            sr.material = onDamageMaterial;
            yield return new WaitForSeconds(onDamageDuration);
            sr.material = originalMaterial;
        }
    }
}