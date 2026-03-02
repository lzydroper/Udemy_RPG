using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    protected Player player;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;
    protected float stateTimer;
    protected bool stateTrigger;

    protected EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        this.player = player;
        this.anim = player.Anim;
        this.rb = player.Rb;
        this.input = player.InputSet;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        stateTrigger = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    private bool CanDash()
    {
        if (player.WallDetected)
            return false;

        if (stateMachine.CurrentState == player.DashState)
            return false;
        
        return true;
    }

    public void CallStateTrigger()
    {
        stateTrigger = true;
    }
}