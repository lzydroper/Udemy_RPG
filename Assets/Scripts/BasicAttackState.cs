using UnityEngine;

public class BasicAttackState : EntityState
{
    private float _basicAttackVelocityTimer;
    
    public BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _basicAttackVelocityTimer = player.basicAttackDuration;
        player.SetVelocity(player.basicAttackVelocity.x * player.facingDir, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        _basicAttackVelocityTimer -= Time.deltaTime;
        if (_basicAttackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
        
        if (stateTrigger)
            stateMachine.ChangeState(player.IdleState);
    }
}