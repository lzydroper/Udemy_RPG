public class JumpAttackState : EntityState
{
    private bool _touchedGround;
    
    public JumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _touchedGround = false;
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!_touchedGround && player.GroundDetected)
        {
            _touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocityY);
        }

        if (stateTrigger && player.GroundDetected)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}