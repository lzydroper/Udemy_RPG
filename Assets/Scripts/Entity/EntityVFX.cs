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
        private Material originalMaterial;
        private Color originalColor;
        private Coroutine onDamageCoroutine;
        private Coroutine onStatusCoroutine;
        [Header("OnTakingDamageVFX")]
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = .2f;
        [Header("OnDoingDamageVFX")]
        [SerializeField] private Color hitVFXColor = Color.white;
        [Header("ElementColor")]
        [SerializeField] private Color iceEffectColor = Color.cyan;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            originalMaterial = sr.material;
            originalColor = sr.color;
        }

        #endregion

        public void PlayOnStatusVFX(float duration, ElementType type)
        {
            if (type == ElementType.None) return;
            if (type == ElementType.Ice) 
                this.RestartCoroutine(ref onStatusCoroutine, OnStatusVFXCo(duration, iceEffectColor));
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

        public void UpdateUpdateOnHitColor(ElementType type)
        {
            hitVFXColor = type switch
            {
                ElementType.Ice => iceEffectColor,
                _ => originalColor,
            };
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