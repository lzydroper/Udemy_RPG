using UnityEngine;

namespace Entity
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Transform damageDealer);
    }
}