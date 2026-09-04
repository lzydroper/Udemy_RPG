using Core.State;
using SkillSystem;
using SkillSystem.Runtime;
using UnityEngine;

namespace Entity.Player.State
{
    public class SwordThrowState : PlayerStateBase
    {

        private Camera camera;
    
        public SwordThrowState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow).EnableDots(true);
            
            if (camera != Camera.main)
                camera = Camera.main;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            var direction = DirectionToMouse();
            player.SetVelocity(0, rb.linearVelocity.y);
            player.HandleFlip(direction.x);
            player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow).PredictTrajectory(direction);
            
            if (player.InputSet.Player.CoAttack.WasReleasedThisFrame())
            {
                ani.SetBool("swordThrowPerform", true);
                player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow).ConfirmedDirection(direction);
            }
            
            // 取消，或完成
            if (player.InputSet.Player.CoAttack.WasPressedThisFrame() || stateTrigger)
                stateMachine.ChangeState(PlayerStateType.Idle);
        }

        public override void OnExit()
        {
            base.OnExit();
            player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow).EnableDots(false);
            ani.SetBool("swordThrowPerform", false);
        }

        private Vector3 DirectionToMouse()
        {
            var playerPos = player.transform.position;
            var mousePos = camera.ScreenToWorldPoint(player.MousePosition);
            var direction = mousePos - playerPos;
            return direction.normalized;
        }
    }
}