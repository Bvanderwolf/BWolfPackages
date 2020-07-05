using BWolf.Utilities.SquadFormations.Selection;
using System.Collections.Generic;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>static class for adding usability functions for managing lists of formation positions, units and selectable objects</summary>
    public static class SquadFormationExtensions
    {
        /// <summary>Returns a sub list of unassigned formation positions</summary>
        public static List<FormationPosition> UnAssigned(this List<FormationPosition> positions)
        {
            List<FormationPosition> unAssigned = new List<FormationPosition>();
            foreach (FormationPosition p in positions)
            {
                if (!p.Assigned) { unAssigned.Add(p); }
            }

            return unAssigned;
        }

        /// <summary>Returns a sub list of unassigned units</summary>
        public static List<Unit> UnAssigned(this List<Unit> units)
        {
            List<Unit> unAssigned = new List<Unit>();
            foreach (Unit u in units)
            {
                if (!u.AssignedPosition) { unAssigned.Add(u); }
            }

            return unAssigned;
        }

        /// <summary>Returns whether this list of units all form a group</summary>
        public static bool FormAGroup(this List<Unit> units)
        {
            if (units.Count <= 1)
            {
                //return false if unit count is smaller than necessary count for making a group
                return false;
            }

            UnitGroup group = UnitGroupHandler.GetGroup(units[0].AssignedGroupId);

            if (group == null || group.EnlistedUnits.Count != units.Count)
            {
                //return false if group doesn't exit or enlisted unit count is not the same as given unit count
                return false;
            }

            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].AssignedGroupId != group.GroupId)
                {
                    //if any units isn't it the group, return false
                    return false;
                }
            }

            return true;
        }

        /// <summary>Converts this list to a list of units by checking for unit components. will be empty when no unit has been found</summary>
        public static List<Unit> ToUnits(this List<SelectableObject> selectables)
        {
            List<Unit> units = new List<Unit>();
            foreach (SelectableObject selectable in selectables)
            {
                Unit u = selectable.GetComponent<Unit>();
                if (u != null)
                {
                    units.Add(u);
                }
            }

            return units;
        }
    }
}