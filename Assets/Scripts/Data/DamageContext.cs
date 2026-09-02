using Entity;
using Stats;
using UnityEngine;

namespace Data
{
    public readonly struct DamageContext
    {
        public Transform DamageDealer { get; }
        public float PhysicDamage { get; }
        public bool IsCritical { get; }
        public float ElementDamage { get; }
        public ElementType ElementType { get; }
        public float ArmorReductionPer { get; }

        public DamageContext(Transform damageDealer, EntityStat entityStat, DamageScaleData damageScale)
        {
            DamageDealer = damageDealer;
            PhysicDamage = entityStat.GetPhysicDamage(out var isCritical, damageScale.physic);
            IsCritical = isCritical;
            ElementDamage = entityStat.GetElementDamage(out var elementType, damageScale.element);
            ElementType = elementType;
            ArmorReductionPer = entityStat.GetArmorReductionPer();
        }
    }
}