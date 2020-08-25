﻿using UnityEngine;

namespace BWolf.Utilities.AgentCommands
{
    public class StateController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        protected StateCenter stateCenter = null;

        [Header("Debug")]
        [SerializeField]
        private State current = null;

        private Agent agent;

        public ICommand CurrentCommand
        {
            get { return agent.CurrentCommand; }
        }

        private void Awake()
        {
            current = stateCenter.Default;
            agent = GetComponent<Agent>();
        }

        private void Update()
        {
            current.Act(this);

            State nextState;
            if (current.TryTransition(this, out nextState))
            {
                Transition(nextState);
            }
        }

        /// <summary>Transitions to given new state</summary>
        public void Transition(State nextState)
        {
            if (nextState != current)
            {
                current = nextState;
            }
        }

        /// <summary>Transitions to a stored state identified by given name</summary>
        public void Transition(string nameofState)
        {
            Transition(stateCenter.GetState(nameofState));
        }

        /// <summary>Sets current state to the stored default state</summary>
        public void SetDefault()
        {
            Transition(stateCenter.Default);
        }
    }
}