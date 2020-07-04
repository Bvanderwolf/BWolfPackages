using BWolf.Examples.SquadFormations.Interactions;
using BWolf.Examples.SquadFormations.Selection;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations
{
    public class Unit : MonoBehaviour
    {
        private NavMeshAgent agent;
        private SelectableObject selectable;
        private FormationPosition assignedPosition;

        public bool AssignedPosition
        {
            get { return assignedPosition != null; }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            selectable = GetComponent<SelectableObject>();
        }

        public void AssignPosition(FormationPosition position)
        {
            assignedPosition = position;
            agent.SetDestination(assignedPosition.Point);
        }

        public void OnInteract(Interaction interaction)
        {
            if (interaction.TypeOfInteraction == InteractionType.MoveOrder)
            {
                Vector3 waypoint = (Vector3)interaction.InteractionContent;
                agent.SetDestination(waypoint);
            }
        }
    }
}