// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PluggableStateMachine
{
    /// <summary>Base Action to be acted out by a state controlled object</summary>
    public abstract class Action : ScriptableObject
    {
        public abstract void Act(StateController controller);
    }
}