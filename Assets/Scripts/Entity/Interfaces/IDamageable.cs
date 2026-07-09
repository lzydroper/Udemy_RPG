using UnityEngine;

namespace Entity.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Transform damageDealer);
    }
}