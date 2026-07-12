using Stats;
using UnityEngine;

namespace Entity.Interfaces
{
    public interface IDamageable
    {
        bool TryTakeDamage(in DamageContext damageContext);
    }
    
    public readonly ref struct DamageContext
    {
        public Transform DamageDealer { get; }
        public float PhysicDamage { get; }
        public float ElementDamage { get; }
        public ElementType ElementType { get; }
        public float ArmorReductionPer { get; }
        
        public DamageContext(Transform damageDealer, float physicDamage, float elementDamage, 
            ElementType elementType, float armorReductionPer)
        {
            DamageDealer = damageDealer;
            PhysicDamage = physicDamage;
            ElementDamage = elementDamage;
            ElementType = elementType;
            ArmorReductionPer = armorReductionPer;
        }
    }
}