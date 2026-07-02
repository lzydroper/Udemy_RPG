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
        public string CurStateKey { get; private set; }
        public string PreStateKey { get; private set; }
        public Action<string, string> OnStateChanged;
        
        public void AddState(string stateKey, IState state)
        {
            m_states[stateKey] = state;
        }
        
        public void Initialize(string stateKey)
        {
            CurStateKey = stateKey;
            CurrentState = m_states[CurStateKey];
            CurrentState.OnEnter();
        }
        
        public void ChangeState(string stateKey)
        {
            CurrentState.OnExit();
            PreStateKey = CurStateKey;
            CurStateKey = stateKey;
            CurrentState = m_states[CurStateKey];
            CurrentState.OnEnter();
            OnStateChanged?.Invoke(PreStateKey, CurStateKey);
        }

        public void OnUpdate()
        {
            CurrentState.OnUpdate();
        }
    }
}