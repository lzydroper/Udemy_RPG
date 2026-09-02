using Core.State;
using SkillSystem;

namespace Entity.Player
{
    public abstract class PlayerStateBase : EntityStateBase
    {
        protected readonly Player player;
        
        protected PlayerStateBase(Player player, StateMachine stateMachine, string animBoolName)
            : base(stateMachine, animBoolName)
        {
            this.player = player;
            this.ani = player.ani;
            this.rb = player.rb;
            this.stat = player.stat;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            if (player.InputSet.Player.Dash.WasPressedThisFrame() && CanDash())
            {
                player.skillManager.GetSkillByType(SkillType.Dash).SetOnCooldown();
                stateMachine.ChangeState(PlayerStateType.Dash);
            }
        }

        protected override void UpdateAnimationParas()
        {
            base.UpdateAnimationParas();
            ani.SetFloat("yVelocity", rb.linearVelocity.y);
        }

        private bool CanDash()
        {
            if (!player.skillManager.GetSkillByType(SkillType.Dash).IsCanUseSkill())
                return false;
            
            if (player.WallDetected)
                return false;

            if (stateMachine.CurStateKey == PlayerStateType.Dash)
                return false;
        
            return true;
        }
    }
}