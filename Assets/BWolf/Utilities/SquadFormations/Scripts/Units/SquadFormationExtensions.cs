using BWolf.Utilities.SquadFormations.Selection;
using System.Collections.Generic;

namespace BWolf.Utilities.SquadFormations.Units
{
    public static class SquadFormationExtensions
    {
        public static List<FormationPosition> UnAssigned(this List<FormationPosition> positions)
        {
            List<FormationPosition> unAssigned = new List<FormationPosition>();
            foreach (FormationPosition p in positions)
            {
                if (!p.Assigned) { unAssigned.Add(p); }
            }

            return unAssigned;
        }

        public static List<Unit> UnAssigned(this List<Unit> units)
        {
            List<Unit> unAssigned = new List<Unit>();
            foreach (Unit u in units)
            {
                if (!u.AssignedPosition) { unAssigned.Add(u); }
            }

            return unAssigned;
        }

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