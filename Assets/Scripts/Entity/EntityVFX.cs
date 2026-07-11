using System.Collections;
using UnityEngine;

namespace Entity
{
    public class EntityVFX : MonoBehaviour
    {
        #region Variables

        private SpriteRenderer sr;
        [Header("OnDamageVFX")]
        [SerializeField] private Material onDamageMaterial;
        [SerializeField] private float onDamageDuration = .2f;
        private Material originalMaterial;
        private Coroutine onDamageCoroutine;

        #endregion

        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            originalMaterial = sr.material;
        }

        #endregion

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