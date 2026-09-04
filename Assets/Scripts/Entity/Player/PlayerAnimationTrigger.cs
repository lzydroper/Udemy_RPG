using SkillSystem;
using SkillSystem.Runtime;

namespace Entity.Player
{
    public class PlayerAnimationTrigger : EntityAnimationTriggers
    {
        #region Variables

        private Player player;

        #endregion

        #region LifecycleMethods

        protected override void Awake()
        {
            base.Awake();
            player = GetComponentInParent<Player>();
        }

        #endregion

        public void SwordThrowTrigger()
        {
            player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow).ThrowSword();
        }
    }
}