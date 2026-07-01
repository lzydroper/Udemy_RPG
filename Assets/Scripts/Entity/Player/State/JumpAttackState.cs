using Core.State;

namespace Entity.Player.State
{
    public class JumpAttackState : PlayerStateBase
    {
        private bool _touchedGround;
    
        public JumpAttackState(Player owner, StateMachine stateMachine, string animBoolName )
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _touchedGround = false;
            player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (!_touchedGround && player.GroundDetected)
            {
                _touchedGround = true;
                player.ani.SetTrigger("jumpAttackTrigger");
                player.SetVelocity(0, player.rb.linearVelocityY);
            }

            if (stateTrigger && player.GroundDetected)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }
    }
}