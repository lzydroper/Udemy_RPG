using Stats;
using UnityEngine;
using VInspector;

namespace Entity
{
    public class EntityStat : MonoBehaviour
    {
        #region Variables

        [Tab("Major Stats")]
        public Stat strength;
        public Stat agility;
        public Stat intelligence;
        public Stat vitality;
        [EndTab]
        [Tab("Offense Stats")]
        // physical damage
        public Stat damage;
        public Stat critPower;
        public Stat critChance;
        public Stat armorReduction;
        // elemental damage
        public Stat fireDamage;
        public Stat iceDamage;
        public Stat lightningDamage;
        [EndTab]
        [Tab("Defense Stats")]
        public Stat maxHealth;
        public Stat evasion;
        public Stat armor;
        public Stat firResistance;
        public Stat iceResistance;
        public Stat lightningResistance;

        [EndTab]

        #endregion

        //TODO:为什么scale要写在这里？
        public float GetElementDamage(out ElementType type, float scale = 1f)
        {
            var elementPairs = new (float damage, ElementType type)[]
            {
                (fireDamage.GetValue(), ElementType.Fire),
                (iceDamage.GetValue(), ElementType.Ice),
                (lightningDamage.GetValue(), ElementType.Lightning)
            };

            float highestElementDamage = 0f;
            type = ElementType.None;
            
            foreach (var item in elementPairs)
            {
                if (item.damage > highestElementDamage)
                {
                    highestElementDamage = item.damage;
                    type = item.type;
                }
            }

            float bonusElementDamage = intelligence.GetValue();
            if (type == ElementType.None) return bonusElementDamage;

            float weakerElementDamage = 0;
            foreach (var item in elementPairs)
            {
                if (item.type != type)
                {
                    weakerElementDamage += item.damage * 0.5f;
                }
            }
            
            float finalDamage = highestElementDamage + weakerElementDamage + bonusElementDamage;
            return finalDamage * scale;
        }

        public float GetElementResistancePer(ElementType type)
        {
            float basicElementResistance = type switch
            {
                ElementType.Fire => firResistance.GetValue(),
                ElementType.Ice => iceResistance.GetValue(),
                ElementType.Lightning => lightningResistance.GetValue(),
                _ => 0
            };
            float bonusElementResistance = intelligence.GetValue() * .5f;
            float totalResistance = basicElementResistance + bonusElementResistance;
            float elementResistanceCap = .75f;
            return Mathf.Clamp(totalResistance / 100f, 0, elementResistanceCap);
        }
        
        public float GetPhysicDamage(out bool isCritical, float scale = 1f)
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
            float finalDamage = isCritical ? totalDamage * totalCritPowerMultiplier : totalDamage;
            return finalDamage * scale;
        }

        // can avoid how much percentage of the damage
        public float GetArmorMitigationPer(float armorReductionPer)
        {
            float basicArmor = armor.GetValue();
            float bonusArmor = vitality.GetValue();
            float totalArmor = basicArmor + bonusArmor;
            
            float armorReductionMultiplier = Mathf.Clamp(1 - armorReductionPer, 0f, 1f);
            float effectiveArmor = totalArmor * armorReductionMultiplier;

            float mitigation = effectiveArmor / (effectiveArmor + 100);
            float mitigationCap = .85f;
            
            return Mathf.Clamp(mitigation, 0, mitigationCap);
        }

        public float GetArmorReductionPer()
        {
            return armorReduction.GetValue() / 100f;
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