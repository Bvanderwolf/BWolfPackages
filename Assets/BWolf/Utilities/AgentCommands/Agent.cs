// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.AgentCommands
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Agent : MonoBehaviour
    {
        public ICommand CurrentCommand { get; private set; }

        private NavMeshAgent agent;
        private Queue<ICommand> commands = new Queue<ICommand>();

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            ProcessCommmands();
        }

        /// <summary>Sets destination for agent to walk towards</summary>
        public void SetDestination(Vector3 worldPosition)
        {
            agent.SetDestination(worldPosition);
        }

        /// <summary>Stops movement of this agent</summary>
        public void StopMovement()
        {
            agent.SetDestination(transform.position);
        }

        /// <summary>Returns whether this agent has reached its destination</summary>
        public bool HasReachedDestination(Vector3 destination)
        {
            if (!agent.pathPending)
            {
                if (agent.destination != destination)
                {
                    //return true if stored destination in the navmeshagent doesn't correspond to destination
                    return true;
                }

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        //return true if agent has reached stored destination and has stopped moving
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Clears commands queued by agent</summary>
        public void ClearCommands()
        {
            commands.Clear();
        }

        /// <summary>Manages command queue process</summary>
        private void ProcessCommmands()
        {
            if (CurrentCommand != null && CurrentCommand.IsFinished())
            {
                //if current command isn't null and is finished check for a new command to deque
                if (commands.Count != 0)
                {
                    SetCurrent(commands.Dequeue());
                }
                else
                {
                    CurrentCommand = null;
                }
            }
        }

        /// <summary>Commands agent using given command with option to override the current active command</summary>
        public void Command(ICommand command, bool overrideCurrent)
        {
            if (overrideCurrent)
            {
                UnSetCurrent();
            }

            if (CurrentCommand != null)
            {
                commands.Enqueue(command);
            }
            else
            {
                SetCurrent(command);
            }
        }

        /// <summary>Undoes the current active command, executing the next queued command if available</summary>
        public void Undo()
        {
            if (CurrentCommand != null)
            {
                UnSetCurrent();
                if (commands.Count != 0)
                {
                    SetCurrent(commands.Dequeue());
                }
            }
        }

        /// <summary>Sets the current command to given command and executes it</summary>
        private void SetCurrent(ICommand command)
        {
            CurrentCommand = command;
            CurrentCommand.Excecute();
        }

        /// <summary>Undoes the current command and nullifies it</summary>
        private void UnSetCurrent()
        {
            if (CurrentCommand != null)
            {
                CurrentCommand.Undo();
                CurrentCommand = null;
            }
        }
    }
}