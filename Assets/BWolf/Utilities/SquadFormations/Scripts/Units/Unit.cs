// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Utilities.SquadFormations.Interactions;
using BWolf.Utilities.SquadFormations.Selection;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Component representing a unit which can be assigned to a group</summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float rePathInterval = 1 / 60f;

        public event Action<Vector3> OnGroupOrder;

        public SelectableObject Selectable { get; private set; }

        /// <summary>Id of group this unit is assigned to. Is -1 if the unit is not part of a group</summary>
        public int AssignedGroupId { get; private set; } = -1;

        public bool IsAtAssignedPosition { get; private set; }

        /// <summary>Is this unit assigned a position in a group</summary>
        public bool IsAssignedAPosition
        {
            get { return assignedPosition != null; }
        }

        private float rePathTime = 0;
        private float sqrMoveDistance;
        private float defaultSpeed;

        private NavMeshAgent agent;
        private FormationPosition assignedPosition;

        /// <summary>The assigned position of this unit. Setting this will also modify how the agent's rotation is updated</summary>
        public FormationPosition AssignedPosition
        {
            get { return assignedPosition; }
            set
            {
                agent.updateRotation = value == null;
                assignedPosition = value;
            }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            Selectable = GetComponent<SelectableObject>();
        }

        private void Start()
        {
            defaultSpeed = agent.speed;

            SetDefaultPriorityValues();
        }

        private void OnDisable()
        {
            ResetValues();
        }

        private void Update()
        {
            if (IsAssignedAPosition)
            {
                RotateTowardsFormationOrientation();

                if (!CheckIfAtAssignedPosition())
                {
                    CheckRepath();
                }
            }
        }

        /// <summary>Resets values related to pathfinding and group movement</summary>
        public void ResetValues()
        {
            rePathTime = 0;
            AssignedPosition = null;
            AssignedGroupId = -1;
            OnGroupOrder = null;

            SetDefaultPriorityValues();
        }

        /// <summary>Assigns a formation position to this unit</summary>
        public void AssignPosition(FormationPosition position)
        {
            AssignedPosition = position;
        }

        /// <summary>Assigns a group id to this unit</summary>
        public void AssignGroupId(int id)
        {
            AssignedGroupId = id;
        }

        public void AssignPrioritySpeed(float speed)
        {
            agent.speed = speed;
        }

        /// <summary>Called when an interaction has been done, it checks for move orders</summary>
        public void OnMoveOrdered(MoveOrderContent content)
        {
            if (content.OrderedUnits.Contains(this))
            {
                if (content.OrderedUnits.Count > 1)
                {
                    OnGroupOrder?.Invoke(content.WayPoint);
                    MoveTowardsAssignedPosition();
                    sqrMoveDistance = (content.WayPoint - transform.position).sqrMagnitude;
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

        /// <summary>Sets the destination of the agent to move towards the assigned position</summary>
        public void MoveTowardsAssignedPosition()
        {
            agent.SetDestination(assignedPosition.Point(false));
        }

        /// <summary>Rotates the unit towards the assigned formation position based on the distance to it when given an order</summary>
        private void RotateTowardsFormationOrientation()
        {
            Vector3 position = transform.position;
            Vector3 point = assignedPosition.Point(false);
            float sqrDistance = (point - position).sqrMagnitude;
            float perc = 1 - sqrDistance / sqrMoveDistance;

            Vector3 lookTarget = Vector3.Lerp(point, assignedPosition.LookPosition, perc);
            float yOrientation = Quaternion.LookRotation(lookTarget - transform.position).eulerAngles.y;
            transform.eulerAngles = new Vector3(0, yOrientation, 0);
        }

        /// <summary>Checks wether the agents destination can be re-set to the assigned position</summary>
        private void CheckRepath()
        {
            rePathTime += Time.deltaTime;
            if (rePathTime >= rePathInterval)
            {
                MoveTowardsAssignedPosition();
                rePathTime = 0;
            }
        }

        /// <summary>Sets IsAtAssignedPosition and returns its value</summary>
        private bool CheckIfAtAssignedPosition()
        {
            IsAtAssignedPosition = (assignedPosition.Point(false) - transform.position).sqrMagnitude < 0.1f;
            return IsAtAssignedPosition;
        }

        /// <summary>Sets default values for a unit without a group</summary>
        private void SetDefaultPriorityValues()
        {
            agent.speed = defaultSpeed;
        }
    }
}