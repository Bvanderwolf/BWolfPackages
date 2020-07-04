using System.Collections.Generic;

namespace BWolf.Utilities.SquadFormations
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
    }
}