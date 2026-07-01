namespace Core.State
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}