using UnityEngine;

namespace Entity.Interfaces
{
    public interface IDamageable
    {
        bool TryTakeDamage(float damage, Transform damageDealer);
    }
}