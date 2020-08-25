namespace BWolf.Utilities.AgentCommands
{
    /// <summary>Base command used by agent behaviours</summary>
    public abstract class AgentCommand : ICommand
    {
        public AgentCommand(Agent agent)
        {
            Agent = agent;
        }

        public Agent Agent { get; private set; }

        public abstract void Excecute();

        public abstract bool IsFinished();

        public abstract void Undo();
    }
}