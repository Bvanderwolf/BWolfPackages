namespace BWolf.Utilities.ObjectPooling
{
    public interface IPoolable
    {
        bool CanTake { get; }

        void Return();

        void OnTake();
    }
}