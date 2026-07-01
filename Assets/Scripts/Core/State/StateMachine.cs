using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.State
{
    [Serializable]
    public class StateMachine
    {
        private readonly Dictionary<string, IState> m_states = new();
        public IState CurrentState { get; private set; }
        [field: SerializeField]
        public string CurrentStateKey { get; private set; }
        public Action<string, string> OnStateChanged;
        
        public void AddState(string stateKey, IState state)
        {
            m_states[stateKey] = state;
        }
        
        public void Initialize(string stateKey)
        {
            CurrentStateKey = stateKey;
            CurrentState = m_states[CurrentStateKey];
            CurrentState.OnEnter();
        }
        
        public void ChangeState(string stateKey)
        {
            CurrentState.OnExit();
            string prevKey = CurrentStateKey;
            CurrentStateKey = stateKey;
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