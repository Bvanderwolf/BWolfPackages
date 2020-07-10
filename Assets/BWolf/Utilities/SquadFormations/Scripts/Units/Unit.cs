using BWolf.Utilities.Flocking.Context;
using BWolf.Utilities.SquadFormations.Interactions;
using BWolf.Utilities.SquadFormations.Selection;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Component representing a unit which can be assigned to a group</summary>
    public class Unit : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Collider unitCollider = null;

        [Header("Settings")]
        [SerializeField]
        private float minSpeed = 3.5f;

        [SerializeField]
        private float maxSpeed = 5f;

        [SerializeField]
        private float rePathInterval = 1 / 60f;

        public event Action<Vector3> OnGroupOrder;

        public SelectableObject Selectable { get; private set; }

        /// <summary>Id of group this unit is assigned to. Is -1 if the unit is not part of a group</summary>
        public int AssignedGroupId { get; private set; } = -1;

        public bool Flockable { get; private set; }

        /// <summary>Is this unit assigned a position in a group</summary>
        public bool IsAssignedAPosition
        {
            get { return assignedPosition != null; }
        }

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

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            Selectable = GetComponent<SelectableObject>();
        }

        private void Start()
        {
            SetDefaultPriorityValues();
        }

        private void OnDisable()
        {
            ResetValues();
        }

        private void Update()
        {
            if (!Flockable && IsAssignedAPosition)
            {
                RotateTowardsFormationOrientation();
                if (!ReachedAssignedPosition())
                {
                    CheckRepath();
                }
                else
                {
                    SetFlocking(true);
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

            SetFlocking(false);
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

        public void AssignPriorityValue(float perc)
        {
            float maxPrioritySpeed = maxSpeed - minSpeed;
            agent.speed = minSpeed + maxPrioritySpeed * perc;
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

        /// <summary>Moves and rotates this unit according to given velocity</summary>
        public void Flock(Vector3 velocity)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
            transform.position += velocity * Time.deltaTime;
        }

        /// <summary>Returns the context of this unit based on given radius represented by a list of context items</summary>
        public List<ContextItem> GetContext(float radius)
        {
            List<ContextItem> context = new List<ContextItem>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c != unitCollider)
                {
                    context.Add(ContextItem.Create(c.transform, c.gameObject.layer));
                }
            }

            return context;
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

        private bool ReachedAssignedPosition()
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
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

        private void SetFlocking(bool value)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = value;
                Flockable = value;
            }
        }

        /// <summary>Sets default values for a unit without a group</summary>
        private void SetDefaultPriorityValues()
        {
            agent.speed = minSpeed;
        }
    }
}