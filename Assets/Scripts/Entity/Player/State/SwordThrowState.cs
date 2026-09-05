using Core.State;
using SkillSystem;
using SkillSystem.Runtime;
using UnityEngine;

namespace Entity.Player.State
{
    public class SwordThrowState : PlayerStateBase
    {

        private Camera camera;
        private SkillSwordThrow swordThrowSkill;
    
        public SwordThrowState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            if (camera != Camera.main)
                camera = Camera.main;
            
            swordThrowSkill ??= player.skillManager.GetSkillByType<SkillSwordThrow>(SkillType.SwordThrow);
            // 为去除残影，提前计算位置再开启
            var direction = DirectionToMouse();
            player.HandleFlip(direction.x);
            swordThrowSkill.PredictTrajectory(direction);
            swordThrowSkill.EnableDots(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            var direction = DirectionToMouse();
            player.SetVelocity(0, rb.linearVelocity.y);
            player.HandleFlip(direction.x);
            swordThrowSkill.PredictTrajectory(direction);
            
            if (player.InputSet.Player.CoAttack.WasReleasedThisFrame())
            {
                ani.SetBool("swordThrowPerform", true);
                swordThrowSkill.ConfirmedDirection(direction);
            }
            
            // 取消，或完成
            if (player.InputSet.Player.CoAttack.WasPressedThisFrame() || stateTrigger)
                stateMachine.ChangeState(PlayerStateType.Idle);
        }

        public override void OnExit()
        {
            base.OnExit();
            swordThrowSkill.EnableDots(false);
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