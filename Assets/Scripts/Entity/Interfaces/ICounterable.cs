namespace Entity.Interfaces
{
    public interface ICounterable
    {
        bool CanBeCountered { get; }
        void HandleCounter();
    }
}