using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.State
{
    [Serializable]
    public class StateMachine<TStateType> 
        where TStateType : Enum
    {
        private readonly Dictionary<TStateType, IState> m_states = new();
        public IState CurrentState { get; private set; }
        [field: SerializeField]
        public TStateType CurrentStateKey { get; private set; }
        public Action<TStateType, TStateType> OnStateChanged;
        
        public void AddState(TStateType key, IState state)
        {
            m_states[key] = state;
        }
        
        public void Initialize(TStateType key)
        {
            CurrentStateKey = key;
            CurrentState = m_states[CurrentStateKey];
            CurrentState.OnEnter();
        }
        
        public void ChangeState(TStateType key)
        {
            CurrentState.OnExit();
            TStateType prevKey = CurrentStateKey;
            CurrentStateKey = key;
            CurrentState = m_states[CurrentStateKey];
            CurrentState.OnEnter();
            OnStateChanged?.Invoke(prevKey, CurrentStateKey);
        }

        public void OnUpdate()
        {
            CurrentState.OnUpdate();
        }
    }
}