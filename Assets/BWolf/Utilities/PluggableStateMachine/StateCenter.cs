// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PluggableStateMachine
{
    /// <summary>Container/Manager class for States controlled by the state controller</summary>
    [CreateAssetMenu(fileName = "StateCenter", menuName = "AgentStateControl/StateCenter")]
    public class StateCenter : ScriptableObject
    {
        [SerializeField]
        private int indexOfDefault = 0;

        [SerializeField]
        private State[] states = null;

        /// <summary>Default state based on indexOfDefault value</summary>
        public State Default
        {
            get { return states[indexOfDefault]; }
        }

        /// <summary>Tries returning a stored state with gaven name. Returns null if none is found</summary>
        public State GetState(string nameofState)
        {
            foreach (State state in states)
            {
                if (state.name == nameofState)
                {
                    return state;
                }
            }

            return null;
        }
    }
}