namespace BWolf.Utilities.AgentCommands
{
    public interface ICommand
    {
        void Excecute();

        void Undo();

        bool IsFinished();
    }
}