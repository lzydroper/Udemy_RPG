using System;
using Core.Pool;
using UnityEngine;
using VInspector;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class VFXController : MonoBehaviour, IPoolRent
    {
        #region Variables

        [SerializeField] private bool randomOffset = true;

        [ShowIf("randomOffset")]
        [SerializeField] private float xMinOffset = -.3f;
        [SerializeField] private float xMaxOffset = 0.3f;
        [SerializeField] private float yMinOffset = -.3f;
        [SerializeField] private float yMaxOffset = 0.3f;
        [EndIf]
        
        [SerializeField] private bool randomRotation = true;

        #endregion

        public void OnRent()
        {
            ApplyRandomOffset();
            ApplyRandomRotation();
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
            var roration = Random.Range(0, 360);
            transform.Rotate(0, 0, roration);
        }
    }
}