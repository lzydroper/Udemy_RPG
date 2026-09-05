using System.Collections;
using Core.Utils;
using Generated;
using UnityEngine;
using VFXs;

namespace Entity.Player
{
    public class PlayerVFX : EntityVFX
    {
        #region Variables

        [Header("TimeEchoVFX")]
        [SerializeField] private float timeEchoInterval = 0.05f;
        private Coroutine timeEchoCoroutine;

        #endregion
        
        public void PlayTimeEchoEffect(float duration) =>
            this.RestartCoroutine(ref timeEchoCoroutine, TimeEchoCo(duration));

        private IEnumerator TimeEchoCo(float duration)
        {
            float timer = 0;
            while (timer < duration)
            {
                if (GameManager.Pool.TryRent<VFXController>(
                        Keys.Pool.DashEchoVFX,
                        out var vfx,
                        position: transform.position,
                        rotation: transform.rotation))
                {
                    vfx.sr.sprite = sr.sprite;
                }
                yield return new WaitForSeconds(timeEchoInterval);
                timer += timeEchoInterval;
            }
        }
    }
}