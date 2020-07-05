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

        public static UnitGroup GetGroup(int id) => groups.ContainsKey(id) ? groups[id] : null;

        /// <summary>Makes given unit leave the group he is in</summary>
        public static void LeaveFromGroup(Unit unit)
        {
            if (groups.ContainsKey(unit.AssignedGroupId))
            {
                UnitGroup g = groups[unit.AssignedGroupId];
                g.RemoveUnit(unit);
                print(g.EnlistedUnits.Count);
                if (!g.TryTrimLastUnit())
                {
                    g.ReAssignUnits();
                }
                print(g.EnlistedUnits.Count);
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