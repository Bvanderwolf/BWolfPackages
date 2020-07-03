using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroup
    {
        private List<Unit> units;
        private List<Vector3> desiredPositions;
        private GameObject centroid;
        private GameObject commander;
        private UnitFormation formation;

        public UnitGroup(List<Unit> enlistedUnits, UnitFormation formation)
        {
            units = new List<Unit>(enlistedUnits);
            desiredPositions = new List<Vector3>(formation.Positions);

            this.formation = formation;
        }
    }
}