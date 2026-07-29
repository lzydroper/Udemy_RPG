using System;
using Entity.Interfaces;
using Stats;
using UnityEngine;

namespace Skills
{
    public abstract class SkillObjBase : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected LayerMask targetLayer;
        [SerializeField] protected Transform targetCheck;
        [SerializeField] protected float checkRadius = 1f;

        #endregion

        protected void DamageInRadius(Transform target, float radius)
        {
            foreach (var tar in GetDetectTargets(target, radius))
            {
                var damageable = tar.GetComponent<IDamageable>();
                if (damageable == null) continue;
                damageable.TryTakeDamage(new DamageContext(transform, 1));
            }
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