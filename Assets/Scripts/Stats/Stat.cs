using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private SerializedDictionary<int, List<StatModifier>> modifierDict = new();
        [SerializeField] private float baseValue;
        private float finalValue;
        private bool needUpdate;

        private void UpdateFinalValue()
        {
            finalValue = baseValue;
            foreach (var modifiers in modifierDict.Values)
            {
                foreach (var modifier in modifiers)
                {
                    finalValue += modifier.value;
                }
            }
            needUpdate = false;
        }

        public float GetValue()
        {
            if (needUpdate)
                UpdateFinalValue();
            return finalValue;
        }

        public void AddModifier(int instanceId, string source, float value)
        {
            if (!modifierDict.ContainsKey(instanceId))
                modifierDict[instanceId] = new List<StatModifier>();
            var modifier = new StatModifier(source, value);
            modifierDict[instanceId].Add(modifier);
            
            needUpdate = true;
        }

        public void RemoveModifier(int instanceId)
        {
            modifierDict.Remove(instanceId);
            
            needUpdate = true;
        }
        
        public void SetBaseValue(float value) => baseValue = value;
    }
    
    [Serializable]
    public class StatModifier
    {
        public string source;
        public float value;
        public StatModifier(string source, float value)
        {
            this.source = source;
            this.value = value;
        }
    }
}