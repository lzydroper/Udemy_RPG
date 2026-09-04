using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillObjSwordPierce : SkillObjSword
    {
        #region Variables

        private int amountOfPierced;

        #endregion

        #region LifecycleMethods

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            bool groundHit = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
            if (amountOfPierced >= skill.amountToPierce || groundHit)
            {
                StopSword(collision);
                DamageInRadius(transform, 0.3f);
                return;
            }
            amountOfPierced++;
            DamageInRadius(transform, 0.3f);
        }

        #endregion
    }
}