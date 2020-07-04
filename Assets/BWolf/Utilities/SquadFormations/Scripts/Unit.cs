using BWolf.Examples.SquadFormations.Interactions;
using BWolf.Examples.SquadFormations.Selection;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations
{
    public class Unit : MonoBehaviour
    {
        [SerializeField]
        private float rePathInterval = 1 / 60f;

        public event Action<Vector3> OnGroupOrder;

        private float rePathTime = 0;
        private bool atAssignedPosition;

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

        private void Update()
        {
            if (!AssignedPosition) { return; }

            CheckAssignedPosition();
            CheckRepath();
        }

        public void AssignPosition(FormationPosition position)
        {
            assignedPosition = position;
            MoveTowardsAssignedPosition();
        }

        public void OnInteract(Interaction interaction)
        {
            if (interaction.TypeOfInteraction == InteractionType.MoveOrder)
            {
                Vector3 waypoint = (Vector3)interaction.InteractionContent;
                if (!AssignedPosition)
                {
                    agent.SetDestination(waypoint);
                }
                else
                {
                    OnGroupOrder?.Invoke(waypoint);
                    atAssignedPosition = false;
                    CheckRepath();
                }
            }
        }

        private void CheckAssignedPosition()
        {
            if (!atAssignedPosition && !agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        atAssignedPosition = true;
                    }
                }
            }
        }

        private void CheckRepath()
        {
            if (atAssignedPosition) { return; }

            rePathTime += Time.deltaTime;
            if (rePathTime >= rePathInterval)
            {
                MoveTowardsAssignedPosition();
                rePathTime = 0;
            }
        }

        private void MoveTowardsAssignedPosition()
        {
            agent.SetDestination(assignedPosition.Point);
        }
    }
}