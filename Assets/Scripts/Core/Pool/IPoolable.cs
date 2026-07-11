namespace Core.Pool
{
    public interface IPoolRent
    {
        void OnRent();
    }
    public interface IPoolReturn
    {
        void OnReturn();
    }
}