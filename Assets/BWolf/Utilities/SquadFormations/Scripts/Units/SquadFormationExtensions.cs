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

        /// <summary>Returns whether this list of units all belong to the same group</summary>
        public static bool IsAGroup(this List<Unit> units)
        {
            if (units.Count <= 1) { return false; }

            int id = units[0].AssignedGroupId;
            for (int i = 1; i < units.Count; i++)
            {
                if (units[i].AssignedGroupId != id)
                {
                    //if any unit doesn't have the same id as the first, return false
                    return false;
                }
            }

            if (id == -1)
            {
                //if all units have an id of -1 it also isn't a group
                return false;
            }
            else
            {
                return true;
            }
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