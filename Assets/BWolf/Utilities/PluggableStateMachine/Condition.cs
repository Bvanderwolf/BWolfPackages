// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PluggableStateMachine
{
    /// <summary>Base condition for a state controlled object to transition to a new state</summary>
    public abstract class Condition : ScriptableObject
    {
        public abstract bool Check(StateController controller);
    }
}