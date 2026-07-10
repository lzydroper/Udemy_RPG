using Core.State;

namespace Entity.Player.State
{
    public class CounterAttackState : PlayerStateBase
    {
        private PlayerCombat _playerCombat;
        private bool hasCountered;
        
        public CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) 
            : base(player, stateMachine, animBoolName)
        {
            _playerCombat = player.GetComponent<PlayerCombat>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            hasCountered = _playerCombat.TryCounterAttack();
            ani.SetBool("counterAttackPerform", hasCountered);
            stateTimer = _playerCombat.CounterDuration;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            player.SetVelocity(0, rb.linearVelocity.y);

            if (stateTrigger)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
            
            if (stateTimer < 0 && !hasCountered)
            {
                stateMachine.ChangeState(PlayerStateType.Idle);
            }
        }
    }
}