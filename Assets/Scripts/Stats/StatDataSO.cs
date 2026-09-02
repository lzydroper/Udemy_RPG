using UnityEngine;
using VInspector;

namespace Stats
{
    [CreateAssetMenu(fileName = "StatDataSO", menuName = "Data/Stat/StatDataSO", order = 0)]
    public class StatDataSO : ScriptableObject
    {
        [Tab("Resources")]
        public float maxHealth = 100;
        public float healthRegen;
        // public float maxMana;
        // public float manaRegen;
        [EndTab]
        [Tab("Major Stats")]
        public float strength;
        public float agility;
        public float intelligence;
        public float vitality;
        [EndTab] 
        [Tab("Offense Stats")] 
        public float attackSpeed = 1;
        // physical damage
        public float damage = 10;
        public float critPower = 150;
        public float critChance;
        public float armorReduction;
        // elemental damage
        public float fireDamage;
        public float iceDamage;
        public float thunderDamage;
        [EndTab]
        [Tab("Defense Stats")]
        public float evasion;
        public float armor;
        public float firResistance;
        public float iceResistance;
        public float thunderResistance;
    }
}