
using UnityEngine;

public class StateMachine
{
    public EntityState CurrentState { get; private set; }

    public void Initialize(EntityState startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        string from = CurrentState.GetType().Name;
        CurrentState.Exit();
        CurrentState = newState;
        string to = CurrentState.GetType().Name;
        CurrentState.Enter();
        Debug.Log($"from : {from}, to: {to}");
    }

    public void UpdateActiveState()
    {
        CurrentState.Update();
    }

    public void CallActiveStateTrigger()
    {
        CurrentState.CallStateTrigger();
    }
}