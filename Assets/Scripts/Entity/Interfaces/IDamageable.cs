using Data;

namespace Entity.Interfaces
{
    public interface IDamageable
    {
        bool TryTakeDamage(in DamageContext damageContext);
    }
}