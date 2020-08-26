using UnityEngine;

namespace BWolf.Utilities.PluggableStates
{
    /// <summary>Structure representing a transition that can be made to a state based on a condition</summary>
    [System.Serializable]
    public struct Transition
    {
#pragma warning disable 0649

        [SerializeField]
        private Condition decision;

        [SerializeField]
        private State state;

#pragma warning restore 0649

        public Condition Decision
        {
            get { return decision; }
        }

        public State State
        {
            get { return state; }
        }
    }
}