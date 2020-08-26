using UnityEngine;

namespace BWolf.Utilities.PluggableStates
{
    /// <summary>Base condition for a state controlled object to transition to a new state</summary>
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Check(StateController controller);
    }
}