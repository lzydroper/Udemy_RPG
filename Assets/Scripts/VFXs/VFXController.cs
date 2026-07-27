using System;
using System.Collections;
using Core.Pool;
using Core.Utils;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

namespace VFXs
{
    public class VFXController : MonoBehaviour, IPoolRent
    {
        #region Variables

        public SpriteRenderer sr;

        [SerializeField] private bool canFade = false;
        [ShowIf("canFade")]
        [SerializeField] private float fadeSpeed = 1f;
        private Coroutine fadeCo;
        [EndIf]

        [SerializeField] private bool randomOffset = true;
        [ShowIf("randomOffset")]
        [SerializeField] private float xMinOffset = -.3f;
        [SerializeField] private float xMaxOffset = 0.3f;
        [SerializeField] private float yMinOffset = -.3f;
        [SerializeField] private float yMaxOffset = 0.3f;
        [EndIf]
        
        [SerializeField] private bool randomRotation = true;
        [ShowIf("randomRotation")]
        [SerializeField] private float minRotation = 0f;
        [SerializeField] private float maxRotation = 360f;
        [EndIf]
        
        #endregion
        
        #region LifecycleMethods

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            StartFadeCo();
        }

        #endregion

        public void OnRent()
        {
            ApplyRandomOffset();
            ApplyRandomRotation();
        }

        private void StartFadeCo()
        {
            if (!canFade) return;
            this.RestartCoroutine(ref fadeCo, FadeCo());
        }

        private IEnumerator FadeCo()
        {
            Color color = Color.white;
            while (color.a > 0f)
            {
                color.a -= fadeSpeed * Time.deltaTime;
                sr.color = color;
                yield return null;
            }
            sr.color = color;
            this.Return();
        }
        
        private void ApplyRandomOffset()
        {
            if (!randomOffset) return;
            var xOffset = Random.Range(xMinOffset, xMaxOffset);
            var yOffset = Random.Range(yMinOffset, yMaxOffset);
            transform.position += new Vector3(xOffset, yOffset);
        }

        private void ApplyRandomRotation()
        {
            if (!randomRotation) return;
            var roration = Random.Range(minRotation, maxRotation);
            transform.Rotate(0, 0, roration);
        }
    }
}