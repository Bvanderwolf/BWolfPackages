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

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            selectable = GetComponent<SelectableObject>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && selectable.IsSelected)
            {
                UnitGroupManager.Instance.StartGroup(this);
            }
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