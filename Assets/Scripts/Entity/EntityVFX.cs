using System.Collections;
using Core.Utils;
using Generated;
using UnityEngine;

namespace Entity
{
    public class EntityVFX : MonoBehaviour
    {
        #region Variables

        private SpriteRenderer sr;
        [Header("OnTakingDamageVFX")]
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = .2f;
        private Material originalMaterial;
        private Coroutine onDamageCoroutine;
        [Header("OnDoingDamageVFX")]
        [SerializeField] private Color hitVFXColor = Color.white;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            originalMaterial = sr.material;
        }

        #endregion

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