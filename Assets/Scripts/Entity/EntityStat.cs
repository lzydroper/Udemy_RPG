using Stats;
using UnityEngine;
using VInspector;

namespace Entity
{
    public class EntityStat : MonoBehaviour
    {
        #region Variables

        [Tab("Major Stats")]
        [SerializeField] private Stat strength;
        [SerializeField] private Stat agility;
        [SerializeField] private Stat intelligence;
        [SerializeField] private Stat vitality;
        [EndTab]
        [Tab("Offense Stats")]
        // physical damage
        [SerializeField] private Stat damage;
        [SerializeField] private Stat critPower;
        [SerializeField] private Stat critChance;
        // elemental damage
        [SerializeField] private Stat fireDamage;
        [SerializeField] private Stat iceDamage;
        [SerializeField] private Stat lightningDamage;
        [EndTab]
        [Tab("Defense Stats")]
        [SerializeField] private Stat maxHealth;
        [SerializeField] private Stat evasion;
        [SerializeField] private Stat armor;
        [SerializeField] private Stat firResistance;
        [SerializeField] private Stat iceResistance;
        [SerializeField] private Stat lightningResistance;

        [EndTab]

        #endregion

        public float GetPhysicalDamage(out bool isCritical)
        {
            float basicDamage = damage.GetValue();
            float bonusDamage = strength.GetValue();
            float totalDamage = basicDamage + bonusDamage;
            
            float basicCritChance = critChance.GetValue();
            float bonusCritChance = agility.GetValue() * .3f;
            float totalCritChance = basicCritChance + bonusCritChance;
            
            float basicCritPower = critPower.GetValue();
            float bonusCritPower = strength.GetValue() * .5f;
            float totalCritPowerMultiplier = (basicCritPower + bonusCritPower) / 100 + 1;
            
            isCritical = Random.Range(0f, 100f) < totalCritChance;
            return isCritical ? totalDamage * totalCritPowerMultiplier : totalDamage;
        }
        
        public float GetMaxHealth()
        {
            float basicHealth = maxHealth.GetValue();
            float bonusHealth = vitality.GetValue() * 5;
            return basicHealth + bonusHealth;
        }

        public float GetEvasion()
        {
            float basicEvasion = evasion.GetValue();
            float bonusEvasion = agility.GetValue() * .5f;
            float totalEvasion = basicEvasion + bonusEvasion;
            float evasionCap = 80f;
            return Mathf.Clamp(totalEvasion, 0, evasionCap);
        }

    }
}