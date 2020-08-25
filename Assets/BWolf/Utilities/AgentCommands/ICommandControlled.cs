namespace BWolf.Utilities.AgentCommands
{
    public interface ICommandControlled
    {
        void Command(ICommand command, bool overrideCurrent);

        void Undo();
    }
}