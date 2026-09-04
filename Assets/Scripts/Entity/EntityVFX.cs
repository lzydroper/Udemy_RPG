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

        protected SpriteRenderer sr;
        // 受到伤害
        [Header("OnTakingDamageVFX")]
        private Coroutine onDamageCoroutine;
        private Material originalMaterial;
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = .2f;
        [Header("ElementColor")]
        private Color originalColor;
        [SerializeField] private Color iceEffectColor = Color.cyan;
        [SerializeField] private Color fireEffectColor = Color.red;
        [SerializeField] private Color thunderColor = Color.yellow;

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

        public Color GetColorByType(ElementType type) => type switch
        {
            ElementType.Fire => fireEffectColor,
            ElementType.Ice => iceEffectColor,
            ElementType.Thunder => thunderColor,
            _ => originalColor
        };

        #endregion

        public void ResetAllVFX()
        {
            StopAllCoroutines();
            sr.material = originalMaterial;
            sr.color = originalColor;
        }

        public void PlayOnStatusVFX(float duration, ElementType type)
        {
            if (type == ElementType.None) return;
            StartCoroutine(OnStatusVFXCo(duration, GetColorByType(type)));
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

        public void CreateOnHitVFX(Transform target, bool isCritical, ElementType type)
        {
            string vfxKey = isCritical ? Keys.Pool.OnCritHitVFX : Keys.Pool.OnHitVFX;
            if (GameManager.Pool.TryRent(vfxKey, out var vfx,
                    position: target.position, parent: target))
            {
                // vfx.GetComponentInChildren<SpriteRenderer>().color = _GetColorByType(type);
                vfx.ReturnAsync(Keys.Pool.OnHitVFX);
            }
        }

        public void PlayOnDamageVFX() 
            => this.RestartCoroutine(ref onDamageCoroutine, OnDamageVFXCo());

        private IEnumerator OnDamageVFXCo()
        {
            sr.material = onDamageMaterial;
            yield return new WaitForSeconds(onDamageDuration);
            sr.material = originalMaterial;
        }
    }
}