using Core.State;

namespace Entity.Player.State
{
    public class DashState : PlayerStateBase
    {
        private float _originalGravityScale;
        private float _dashDir;

        public DashState(Player owner, StateMachine stateMachine, string animBoolName)
            : base(owner, stateMachine, animBoolName)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _dashDir = (player.MoveInput.x != 0) ? player.MoveInput.x : player.facingDir;
            _originalGravityScale = player.rb.gravityScale;
            player.rb.gravityScale = 0;

            stateTimer = player.dashDuration;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            // cancel dash logic
            if (player.WallDetected)
            {
                if (player.GroundDetected)
                {
                    stateMachine.ChangeState(PlayerStateType.Idle);
                }
                else
                {
                    stateMachine.ChangeState(PlayerStateType.WallSlide);
                }
            }
        
            player.SetVelocity(_dashDir * player.dashSpeed, 0);

            if (stateTimer < 0)
            {
                if (player.GroundDetected)
                {
                    stateMachine.ChangeState(PlayerStateType.Idle);
                }
                else
                {
                    stateMachine.ChangeState(PlayerStateType.Fall);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        
            player.SetVelocity(0, 0);
            player.rb.gravityScale = _originalGravityScale;
        
        }
    }
}