using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DamageScaleData
    {
        [Header("Damage")]
        public float physic = 1;
        public float element = 1;
    
        [Header("Ice")]
        public float iceDuration = 3;
        public float iceSlowMultiplier = 0.5f;
    
        [Header("Fire")]
        public float fireDuration =3;
        public float fireTotalDamageScale = 1;
    
        [Header("Thunder")]
        public float thunderDuration = 3;
        public float thunderDamageScale = 1;
        public float thunderCharge = 0.4f;
    }
}