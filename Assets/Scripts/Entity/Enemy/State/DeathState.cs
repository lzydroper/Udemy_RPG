using Core.State;
using UnityEngine;

namespace Entity.Enemy.State
{
    public class DeathState : EnemyStateBase
    {
        private Collider2D col;
        public DeathState(EnemyBase enemy, StateMachine stateMachine, string animBoolName = null) 
            : base(enemy, stateMachine, animBoolName)
        {
            col = enemy.GetComponent<Collider2D>();
        }

        public override void OnEnter()
        {
            ani.enabled = false;
            col.enabled = false;

            rb.gravityScale = 12f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 20f);
            
            stateMachine.SwitchOffStateMachine();
        }
    }
}