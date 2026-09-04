using Core.State;
using SkillSystem;

namespace Entity.Player.State
{
    public abstract class GroundedState : PlayerStateBase
    {
        protected GroundedState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            if (rb.linearVelocity.y < 0 && !player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Fall);
            }

            if (player.InputSet.Player.Jump.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.Jump);
            }

            if (player.InputSet.Player.BasicAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.BasicAttack);
            }

            if (player.InputSet.Player.CounterAttack.WasPressedThisFrame())
            {
                stateMachine.ChangeState(PlayerStateType.CounterAttack);
            }

            if (player.InputSet.Player.CoAttack.WasPressedThisFrame() && 
                player.skillManager.GetSkillByType(SkillType.SwordThrow).IsCanUseSkill())
            {
                stateMachine.ChangeState(PlayerStateType.SwordThrow);
            }
        }
    }
}