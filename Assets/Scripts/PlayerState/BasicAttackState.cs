using UnityEngine;

public class BasicAttackState : EntityState
{
    private float _basicAttackVelocityTimer;
    private float _lastAttackTime;
    
    private bool _comboAttackQueued;
    private int _comboIndex;
    private int _comboLimit;
    private float _attackDir;
    
    public BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (_comboLimit != player.basicAttackVelocity.Length)
        {
            _comboLimit = player.basicAttackVelocity.Length;
            Debug.LogWarning("adjust combo limit according to basic attack velocity array");
        }
    }

    public override void Enter()
    {
        base.Enter();
        // reset combo
        _comboAttackQueued = false;
        if (Time.time > _lastAttackTime + player.comboAttackTime)
        {
            _comboIndex = 0;
        }
        _comboIndex %= _comboLimit;
        _attackDir = (player.MoveInput.x != 0) ? player.MoveInput.x : player.facingDir; // works as when attacking but move other side
        
        // apply attack velocity
        _basicAttackVelocityTimer = player.basicAttackDuration;
        Vector2 attackVelocity = player.basicAttackVelocity[_comboIndex];
        player.SetVelocity(attackVelocity.x * _attackDir, attackVelocity.y);
        
        // set anim
        anim.SetInteger("basicAttackIndex", _comboIndex);
    }

    public override void Update()
    {
        base.Update();
        
        // handle attack velocity
        _basicAttackVelocityTimer -= Time.deltaTime;
        if (_basicAttackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
        
        // handle attack combo
        if (input.Player.BasicAttack.WasPressedThisFrame())
        {
            if (_comboIndex < _comboLimit)
            {
                _comboAttackQueued = true;
            }
        }
        
        if (stateTrigger)
        {
            if (_comboAttackQueued)
            {
                anim.SetBool(animBoolName, false);
                player.EnterAttackStateWithDelay();
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        _comboIndex += 1;
        _lastAttackTime = Time.time;
    }
}