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

        public float GetMaxHealth()
        {
            float baseHealth = maxHealth.GetValue();
            float bonusHealth = vitality.GetValue() * 5;
            return baseHealth + bonusHealth;
        }

        public float GetEvasion()
        {
            float baseEvasion = evasion.GetValue();
            float bonusEvasion = agility.GetValue() * .5f;
            float totalEvasion = baseEvasion + bonusEvasion;
            float evasionCap = 80f;
            return Mathf.Clamp(totalEvasion, 0, evasionCap);
        }

    }
}