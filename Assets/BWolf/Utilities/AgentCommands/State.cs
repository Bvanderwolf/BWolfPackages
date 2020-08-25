using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.AgentCommands
{
    /// <summary>State Controlled by the state controlled</summary>
    [CreateAssetMenu(fileName = "AgentState", menuName = "AgentStateControl/State")]
    public class State : ScriptableObject
    {
        [SerializeField]
        private List<Action> actions = new List<Action>();

        [SerializeField]
        private List<Transition> transitions = new List<Transition>();

        /// <summary>Acts out stored actions</summary>
        public void Act(StateController controller)
        {
            foreach (Action action in actions)
            {
                action.Act(controller);
            }
        }

        /// <summary>Tries transitioning by checking conditions. ouputs new state if succesfull</summary>
        public bool TryTransition(StateController controller, out State newState)
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].Decision.Check(controller))
                {
                    newState = transitions[i].State;
                    return true;
                }
            }

            newState = null;
            return false;
        }
    }
}