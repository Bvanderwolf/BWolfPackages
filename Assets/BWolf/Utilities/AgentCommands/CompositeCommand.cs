using System.Collections.Generic;

namespace BWolf.Utilities.AgentCommands
{
    /// <summary>Composition class making it possible to express one command as multiple queued ones</summary>
    public class CompositeCommand : ICommand
    {
        private Queue<ICommand> _commands;
        private ICommand _currentCommand;

        /// <summary>Creates a new composition object. Order in queue is based on params order</summary>
        public CompositeCommand(params ICommand[] commands)
        {
            _commands = new Queue<ICommand>();

            foreach (ICommand command in commands)
            {
                _commands.Enqueue(command);
            }
        }

        /// <summary>deques commands and executes new active one</summary>
        public void Excecute()
        {
            _currentCommand = _commands.Dequeue();
            _currentCommand.Excecute();
        }

        /// <summary>Returns whether all commands have been dequed and there is none active</summary>
        public bool IsFinished()
        {
            if (_currentCommand.IsFinished())
            {
                _currentCommand = null;

                if (_commands.Count == 0)
                {
                    return true;
                }
                else
                {
                    Excecute();
                }
            }

            return false;
        }

        /// <summary>clears queued commands</summary>
        public void Undo()
        {
            _currentCommand.Undo();
            _currentCommand = null;

            _commands.Clear();
        }
    }
}