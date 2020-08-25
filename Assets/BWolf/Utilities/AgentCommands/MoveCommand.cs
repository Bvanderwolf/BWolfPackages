using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.AgentCommands
{
    /// <summary>Move command for an agent to move towards a location on a nav mesh</summary>
    public class MoveCommand : AgentCommand
    {
        private const float NAVMESH_SAMPLE_DISTANCE = 5f;
        private Vector3 _destination;

        public MoveCommand(Agent agent, Vector3 destination) : base(agent)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(destination, out hit, NAVMESH_SAMPLE_DISTANCE, NavMesh.AllAreas))
            {
                _destination = hit.position;
            }
        }

        /// <summary>Sets the destination of the agent based on stored destination</summary>
        public override void Excecute()
        {
            Agent.SetDestination(_destination);
        }

        /// <summary>Checks whether the destination of the move command has been reached</summary>
        public override bool IsFinished()
        {
            return Agent.HasReachedDestination(_destination);
        }

        /// <summary>Stops the movement of the agent</summary>
        public override void Undo()
        {
            Agent.StopMovement();
        }
    }
}