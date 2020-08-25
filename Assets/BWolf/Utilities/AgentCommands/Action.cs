using UnityEngine;

namespace BWolf.Utilities.AgentCommands
{
    /// <summary>Base Action to be acted out by a state controlled object</summary>
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}