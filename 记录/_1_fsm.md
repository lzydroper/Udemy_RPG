# 1.FSM 有限状态机

定义状态接口，状态必须有通用的函数

```csharp
public interface IState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    void CallStateTrigger();
}
```

状态关键字用string保证状态可以复用，定义statemachine内部流转

```csharp
public class StateMachine
{
    private readonly Dictionary<string, IState> m_states = new();
    public IState CurrentState { get; private set; }
    public string CurStateKey { get; private set; }
    
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
        CurStateKey = stateKey;
        CurrentState = m_states[CurStateKey];
        CurrentState.OnEnter();
    }

    public void OnUpdate()
    {
        CurrentState.OnUpdate();
    }
}
```

使用string作为key而不是泛型enum：保证泛用性和可复用性，通过集中定义statekey来保证拼写检查和集中改动

不将具体的state集中放入实体类中，而是让state内部自己处理流转，实体类只负责注册和初始化

```csharp
namespace Entity.Player
{
    public static class PlayerStateType
    {
        public const string Idle = "Idle";
        public const string Move = "Move";
        public const string Jump = "Jump";
        public const string Fall = "Fall";
        public const string WallSlide = "WallSlide";
        public const string WallJump = "WallJump";
        public const string Dash = "Dash";
        public const string BasicAttack = "BasicAttack";
        public const string JumpAttack = "JumpAttack";
        public const string Death = "Death";
        public const string CounterAttack = "CounterAttack";
    }
}
```

