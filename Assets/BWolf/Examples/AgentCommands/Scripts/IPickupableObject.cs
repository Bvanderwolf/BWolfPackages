namespace BWolf.Examples.AgentCommands
{
    public interface IPickupable
    {
        void StartPickup();

        void UndoPickup();

        bool IsPickedUp();
    }
}