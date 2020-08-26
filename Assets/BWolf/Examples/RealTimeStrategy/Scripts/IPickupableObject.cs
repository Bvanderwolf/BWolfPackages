namespace BWolf.Examples.RealTimeStrategy
{
    public interface IPickupable
    {
        void StartPickup();

        void UndoPickup();

        bool IsPickedUp();
    }
}