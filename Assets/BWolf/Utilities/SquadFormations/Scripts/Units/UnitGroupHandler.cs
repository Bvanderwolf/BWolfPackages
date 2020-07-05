using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>class for handling the different groups that may be formed by the user</summary>
    public class UnitGroupHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabFormation = null;

        private static readonly Dictionary<int, UnitGroup> groups = new Dictionary<int, UnitGroup>();

        /// <summary>Makes given unit leave the group he is in</summary>
        public static void LeaveFromGroup(Unit unit)
        {
            if (groups.ContainsKey(unit.AssignedGroupId))
            {
                UnitGroup g = groups[unit.AssignedGroupId];
                g.RemoveUnit(unit);
                g.ReAssignUnits();
            }
        }

        /// <summary>Starts a new group using given units</summary>
        public void StartGroup(List<Unit> units)
        {
            CleanActiveGroups(units);

            UnitGroup group;
            if (!TryGetGroup(out group))
            {
                int id = groups.Count;
                group = new UnitGroup(id, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                groups.Add(id, group);
            }

            group.AssignUnitsToGroup(units);
        }

        /// <summary>Starts a new group using given units, also setting the formation position aswell</summary>
        public void StartGroup(List<Unit> units, Vector3 formationPosition)
        {
            CleanActiveGroups(units);

            UnitGroup group;
            if (!TryGetGroup(out group))
            {
                int id = groups.Count;
                group = new UnitGroup(id, Instantiate(prefabFormation).GetComponent<UnitFormation>());
                groups.Add(id, group);
            }

            group.AssignUnitsToGroup(units, formationPosition);
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

            //clean groups with only 1 unit in them
            foreach (UnitGroup g in groups.Values)
            {
                if (g.EnlistedUnits.Count == 1)
                {
                    g.EnlistedUnits.RemoveAt(0);
                }
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