// Created By: Benjamin van der Wolf
// Version: 1.1
//----------------------------------

namespace BWolf.Utilities.AgentCommands
{
    /// <summary>Base command used by agent behaviours</summary>
    public abstract class AgentCommand : ICommand
    {
        public readonly Agent Agent;

        public AgentCommand(Agent agent)
        {
            Agent = agent;
        }

        public abstract void Excecute();

        public abstract bool IsFinished();

        public abstract void Undo();
    }
}