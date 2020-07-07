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

        private NavMeshAgent agent;
        private FormationPosition assignedPosition;

        public SelectableObject Selectable { get; private set; }

        public int AssignedGroupId { get; private set; } = -1;

        public bool IsAssignedAPosition
        {
            get { return assignedPosition != null; }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            Selectable = GetComponent<SelectableObject>();
        }

        private void OnDisable()
        {
            ResetValues();
        }

        private void Update()
        {
            if (!IsAssignedAPosition) { return; }

            CheckRepath();
        }

        /// <summary>Resets values related to pathfinding and group movement</summary>
        public void ResetValues()
        {
            rePathTime = 0;
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
                if (content.OrderedUnits.Contains(this))
                {
                    if (content.OrderedUnits.Count > 1)
                    {
                        OnGroupOrder?.Invoke(content.WayPoint);
                        MoveTowardsAssignedPosition();
                    }
                    else
                    {
                        if (IsAssignedAPosition)
                        {
                            UnitGroupHandler.LeaveFromGroup(this);
                        }
                        agent.SetDestination(content.WayPoint);
                    }
                }
            }
        }

        /// <summary>Sets the destination of the agent to move towards the assigned position</summary>
        public void MoveTowardsAssignedPosition()
        {
            agent.SetDestination(assignedPosition.Point(false));
        }

        /// <summary>Checks wether the agents destination can be re-set to the assigned position</summary>
        private void CheckRepath()
        {
            if (transform.position == assignedPosition.Point(false)) { return; }

            rePathTime += Time.deltaTime;
            if (rePathTime >= rePathInterval)
            {
                MoveTowardsAssignedPosition();
                rePathTime = 0;
            }
        }
    }
}