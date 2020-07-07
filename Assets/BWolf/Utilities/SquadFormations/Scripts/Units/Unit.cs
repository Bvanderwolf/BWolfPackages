﻿using BWolf.Utilities.SquadFormations.Interactions;
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
        private float sqrMoveDistance;

        private NavMeshAgent agent;
        private FormationPosition assignedPosition;

        private FormationPosition AssignedPosition
        {
            get { return assignedPosition; }
            set
            {
                agent.updateRotation = value == null;
                assignedPosition = value;
            }
        }

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

            RotateTowardsFormationOrientation();
            CheckRepath();
        }

        /// <summary>Resets values related to pathfinding and group movement</summary>
        public void ResetValues()
        {
            rePathTime = 0;
            AssignedPosition = null;
            AssignedGroupId = -1;
            OnGroupOrder = null;
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