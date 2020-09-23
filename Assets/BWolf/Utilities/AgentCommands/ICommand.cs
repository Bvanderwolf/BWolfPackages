// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

namespace BWolf.Utilities.AgentCommands
{
    public interface ICommand
    {
        void Excecute();

        void Undo();

        bool IsFinished();
    }
}