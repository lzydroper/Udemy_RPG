using System.Collections;
using Core.State;
using UnityEngine;

namespace Entity.Player.State
{
    public class BasicAttackState : PlayerStateBase
    {
        private float _basicAttackVelocityTimer;
        private float _lastAttackTime;
    
        private bool _comboAttackQueued;
        private int _comboIndex;
        private int _comboLimit;
        private float _attackDir;
        private Coroutine _queuedAttackCo;
    
        public BasicAttackState(Player player, StateMachine stateMachine, string animBoolName )
            : base(player, stateMachine, animBoolName)
        {
            _comboLimit = player.basicAttackVelocity.Length;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            // reset combo
            _comboAttackQueued = false;
            if (Time.time > _lastAttackTime + player.comboAttackTime)
            {
                _comboIndex = 0;
            }
            _comboIndex %= _comboLimit;
            _attackDir = (player.MoveInput.x != 0) ? player.MoveInput.x : player.facingDir; // works as when attacking but move other side
        
            UpdateAttackSpeed();
            
            // apply attack velocity
            _basicAttackVelocityTimer = player.basicAttackDuration;
            Vector2 attackVelocity = player.basicAttackVelocity[_comboIndex];
            player.SetVelocity(attackVelocity.x * _attackDir, attackVelocity.y);
        
            // set anim
            ani.SetInteger("basicAttackIndex", _comboIndex);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        
            // handle attack velocity
            _basicAttackVelocityTimer -= Time.deltaTime;
            if (_basicAttackVelocityTimer < 0)
                player.SetVelocity(0, rb.linearVelocity.y);
        
            // handle attack combo
            if (player.InputSet.Player.BasicAttack.WasPressedThisFrame())
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
                    ani.SetBool(animBoolName, false);
                    EnterAttackStateWithDelay();
                }
                else
                {
                    stateMachine.ChangeState(PlayerStateType.Idle);
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            _comboIndex += 1;
            _lastAttackTime = Time.time;
        }
    
        private void EnterAttackStateWithDelay()
        {
            if (_queuedAttackCo != null)
            {
                player.StopCoroutine(_queuedAttackCo);
            }
            _queuedAttackCo = player.StartCoroutine(EnterAttackStateWithDelayCo());
        }
        private IEnumerator EnterAttackStateWithDelayCo()
        {
            yield return new WaitForEndOfFrame();
            stateMachine.ChangeState(PlayerStateType.BasicAttack);
        }
    }
}