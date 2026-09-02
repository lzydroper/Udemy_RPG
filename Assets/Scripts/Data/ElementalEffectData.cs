using System;
using Entity;
using Stats;

namespace Data
{
    [Serializable]
    public class ElementalEffectData
    {
        public float iceDuration;
        public float iceSlowMultiplier;
    
        public float fireDuration;
        public float fireTotalDamage;
    
        public float thunderDuration;
        public float thunderDamage;
        public float thunderCharge;

        public ElementalEffectData(EntityStat entityStat, DamageScaleData damageScale)
        {
            iceDuration = damageScale.iceDuration;
            iceSlowMultiplier = damageScale.iceSlowMultiplier;
        
            fireDuration = damageScale.fireDuration;
            fireTotalDamage = entityStat.fireDamage.GetValue() * damageScale.fireTotalDamageScale;
        
            thunderDuration = damageScale.thunderDuration;
            thunderDamage = entityStat.thunderDamage.GetValue() * damageScale.thunderDamageScale;
            thunderCharge = damageScale.thunderCharge;
        }
    }
}