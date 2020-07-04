using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroup
    {
        private List<Unit> units;
        private GameObject centroid;
        private GameObject commander;
        private UnitFormation formation;

        public UnitGroup(List<Unit> enlistedUnits, UnitFormation formation)
        {
            units = new List<Unit>(enlistedUnits);

            this.formation = formation;
            this.formation.AssignUnitPositions(units);
        }
    }
}