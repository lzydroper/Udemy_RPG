using Stats;
using UnityEngine;
using VInspector;

namespace Entity
{
    public class EntityStat : MonoBehaviour
    {
        #region Variables

        [Foldout("Health")]
        [SerializeField] private Stat maxHealth;
        [SerializeField] private Stat vitality;
        [EndFoldout]

        #endregion

        public float GetMaxHealth()
        {
            float baseHealth = maxHealth.GetValue();
            float bonusHealth = vitality.GetValue() * 5;
            return baseHealth + bonusHealth;
        }

    }
}