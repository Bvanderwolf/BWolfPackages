using BWolf.Examples.SquadFormations.Interactions;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations
{
    public class Unit : MonoBehaviour
    {
        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
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