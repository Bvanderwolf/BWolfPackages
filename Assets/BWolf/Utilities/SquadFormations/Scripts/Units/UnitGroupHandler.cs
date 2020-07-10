using BWolf.Utilities.Flocking.Context;
using BWolf.Utilities.SquadFormations.Flocking;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>class for handling the different groups that may be formed by the user</summary>
    public class UnitGroupHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject prefabFormation = null;

        [Header("Flock Settings/References")]
        [SerializeField]
        private SquadFlockBehaviour groupBehaviour = null;

        [SerializeField]
        private float driveFactor = 10f;

        [SerializeField]
        private float maxSpeed = 5f;

        [SerializeField]
        private float neighbourRadius = 3f;

        [SerializeField]
        private float avoidanceRadiusMultiplier = 0.75f;

        [HideInInspector]
        public Vector3 SquadVelocity;

        public float SqrAvoidanceRadius { get; private set; }

        private float sqrMaxSpeed;

        private static readonly Dictionary<int, UnitGroup> groups = new Dictionary<int, UnitGroup>();

        public static UnitGroup GetGroup(int id) => groups.ContainsKey(id) ? groups[id] : null;

        private void Awake()
        {
            sqrMaxSpeed = maxSpeed * maxSpeed;
            SqrAvoidanceRadius = (neighbourRadius * neighbourRadius) * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        }

        private void Update()
        {
            List<Unit> flock;
            foreach (UnitGroup group in groups.Values)
            {
                if (group.TryGetFlock(out flock))
                {
                    foreach (Unit unit in flock)
                    {
                        List<ContextItem> context = unit.GetContext(neighbourRadius);
                        Vector3 step = groupBehaviour.CalculateStep(unit, context, this);
                        step *= driveFactor;
                        if (step.sqrMagnitude > sqrMaxSpeed)
                        {
                            step = step.normalized * maxSpeed;
                        }
                        step.y = 0; //make unit clamp position to navmesh using agent.sample navmeshpositoin
                        unit.Flock(step);
                    }
                }
            }
        }

        /// <summary>Makes given unit leave the group he is in</summary>
        public static void LeaveFromGroup(Unit unit)
        {
            if (groups.ContainsKey(unit.AssignedGroupId))
            {
                UnitGroup g = groups[unit.AssignedGroupId];
                g.RemoveUnit(unit);
                if (!g.TryTrimLastUnit())
                {
                    g.ReAssignUnits();
                }
            }
        }

        /// <summary>Returns groups of which atleast one unit is selected. If no units are selected returns null.</summary>
        public static List<UnitGroup> GetSelectedGroups()
        {
            List<UnitGroup> selectedGroups = new List<UnitGroup>();
            foreach (UnitGroup group in groups.Values)
            {
                foreach (Unit unit in group.EnlistedUnits)
                {
                    if (unit.Selectable.IsSelected)
                    {
                        selectedGroups.Add(group);
                        break;
                    }
                }
            }
            return selectedGroups;
        }

        /// <summary>Starts a new group using given units, also setting the formation position aswell</summary>
        public List<Unit> StartGroup(List<Unit> units, Vector3 formationPosition)
        {
            CleanActiveGroups(units);

            UnitGroup group;
            if (!TryGetGroup(out group))
            {
                int id = groups.Count;
                group = new UnitGroup(id, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                groups.Add(id, group);
            }

            return group.AssignUnitsToGroup(units, formationPosition);
        }

        /// <summary>Cleans the active groups of given units</summary>
        private void CleanActiveGroups(List<Unit> units)
        {
            //remove units from active groups and reset their values
            foreach (Unit unit in units)
            {
                if (groups.ContainsKey(unit.AssignedGroupId))
                {
                    groups[unit.AssignedGroupId].RemoveUnit(unit);
                }

                unit.ResetValues();
            }

            //try trimming groups with only one unit left
            foreach (UnitGroup g in groups.Values)
            {
                g.TryTrimLastUnit();
            }
        }

        /// <summary>Tries outputting a group that is not used, returns whether it has succeeded or not</summary>
        private bool TryGetGroup(out UnitGroup group)
        {
            if (groups.Count == 0)
            {
                group = null;
                return false;
            }

            group = groups.Values.FirstOrDefault(g => g.EnlistedUnits.Count == 0);

            return group != null;
        }
    }
}