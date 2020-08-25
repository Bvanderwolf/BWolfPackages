namespace BWolf.Utilities.AgentCommands
{
    public interface IPickupable
    {
        void StartPickup();

        void UndoPickup();

        bool IsPickedUp();
    }
}