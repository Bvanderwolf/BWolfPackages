using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitGroup
    {
        private List<Unit> units;
        private Unit commander;
        private UnitFormation formation;

        public UnitGroup(List<Unit> enlistedUnits, UnitFormation formation)
        {
            units = new List<Unit>(enlistedUnits);
            commander = formation.AssignUnitPositions(units);
            commander.OnGroupOrder += OnGroupOrder;

            this.formation = formation;
        }

        private void OnGroupOrder(Vector3 wayPoint)
        {
            formation.transform.position = wayPoint;
        }
    }
}