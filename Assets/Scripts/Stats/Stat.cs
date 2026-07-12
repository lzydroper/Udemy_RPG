using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private float baseValue;

        public float GetValue()
        {
            return baseValue;
        }
    }
}