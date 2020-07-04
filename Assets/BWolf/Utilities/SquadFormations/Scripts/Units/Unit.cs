using BWolf.Utilities.SquadFormations.Interactions;
using BWolf.Utilities.SquadFormations.Selection;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations.Units
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

        public int AssignedGroupId { get; private set; } = -1;

        public bool AssignedPosition
        {
            get { return assignedPosition != null; }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            selectable = GetComponent<SelectableObject>();
        }

        private void OnDisable()
        {
            ResetValues();
        }

        private void Update()
        {
            if (!AssignedPosition) { return; }

            CheckAtAssignedPosition();
            CheckRepath();
        }

        public void ResetValues()
        {
            rePathTime = 0;
            atAssignedPosition = false;
            assignedPosition = null;
            AssignedGroupId = -1;
            OnGroupOrder = null;
        }

        public void AssignPosition(FormationPosition position)
        {
            assignedPosition = position;
        }

        public void AssignGroupId(int id)
        {
            AssignedGroupId = id;
        }

        public void OnInteract(Interaction interaction)
        {
            if (interaction.TypeOfInteraction == InteractionType.MoveOrder)
            {
                MoveOrderContent content = (MoveOrderContent)interaction.InteractionContent;
                if (!content.GroupMove)
                {
                    agent.SetDestination(content.WayPoint);
                }
                else
                {
                    OnGroupOrder?.Invoke(content.WayPoint);
                    MoveTowardsAssignedPosition();
                    atAssignedPosition = false;
                }
            }
        }

        private void CheckAtAssignedPosition()
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