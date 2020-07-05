using BWolf.Utilities.SquadFormations.Interactions;
using BWolf.Utilities.SquadFormations.Selection;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Component representing a unit which can be assigned to a group</summary>
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

        /// <summary>Resets values related to pathfinding and group movement</summary>
        public void ResetValues()
        {
            rePathTime = 0;
            atAssignedPosition = false;
            assignedPosition = null;
            AssignedGroupId = -1;
            OnGroupOrder = null;
        }

        /// <summary>Assigns a formation position to this unit</summary>
        public void AssignPosition(FormationPosition position)
        {
            assignedPosition = position;
        }

        /// <summary>Assigns a group id to this unit</summary>
        public void AssignGroupId(int id)
        {
            AssignedGroupId = id;
        }

        /// <summary>Called when an interaction has been done, it checks for move orders</summary>
        public void OnInteract(Interaction interaction)
        {
            if (interaction.TypeOfInteraction == InteractionType.MoveOrder)
            {
                MoveOrderContent content = (MoveOrderContent)interaction.InteractionContent;
                if (!content.GroupMove)
                {
                    if (AssignedPosition)
                    {
                        UnitGroupHandler.LeaveFromGroup(this);
                        ResetValues();
                    }
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

        /// <summary>Checks whether the unit has reached its assigned position</summary>
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

        /// <summary>Checks wether the agents destination can be re-set to the assigned position</summary>
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

        /// <summary>Sets the destination of the agent to move towards the assigned position</summary>
        private void MoveTowardsAssignedPosition()
        {
            agent.SetDestination(assignedPosition.Point);
        }
    }
}