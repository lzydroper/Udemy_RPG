using Data;
using Entity;
using Entity.Interfaces;
using Stats;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public abstract class SkillObjBase : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Transform targetCheck;
        [SerializeField] protected float checkRadius = 1f;
        [SerializeField] protected float closestCheckR = 10f;
        
        protected EntityStat entityStat;
        protected EntityVFX entityVFX;
        protected DamageScaleData damageScale;

        #endregion

        protected void DamageInRadius(Transform target, float radius, out ElementType elementType)
        {
            var effectData = new ElementalEffectData(entityStat, damageScale);
            var context = new DamageContext(transform, entityStat, damageScale);
            elementType = context.ElementType;
            
            foreach (var tar in GetDetectTargets(target, radius))
            {
                var damageable = tar.GetComponent<IDamageable>();
                if (damageable == null) continue;
                
                if (damageable.TryTakeDamage(context))
                {
                    if (elementType != ElementType.None) 
                        tar.GetComponent<EntityStatusHandler>()?.ApplyElementEffect(elementType, effectData);
                    entityVFX?.CreateOnHitVFX(tar.transform, context.IsCritical, context.ElementType);
                }
            }
        }

        protected Transform GetClosestTarget(Transform source, float radius)
        {
            Transform target = null;
            float closestDistance = Mathf.Infinity;
            foreach (var tar in GetDetectTargets(source, radius))
            {
                float distance = Vector2.Distance(source.position, tar.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = tar.transform;
                }
            }
            return target;
        }
        
        protected Collider2D[] GetDetectTargets(Transform target, float radius)
        {
            return Physics2D.OverlapCircleAll(target.position, radius, targetLayer);
        }

        protected void OnDrawGizmos()
        {
            if (targetCheck == null) targetCheck = transform;
            Gizmos.DrawWireSphere(targetCheck.position, checkRadius);
        }
    }
}