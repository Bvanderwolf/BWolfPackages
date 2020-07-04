using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    public class UnitGroup
    {
        public readonly List<Unit> EnlistedUnits = new List<Unit>();
        public readonly int GroupId;

        private Unit commander;
        private UnitFormation formation;

        public UnitGroup(int id, List<Unit> enlistedUnits, UnitFormation formation)
        {
            GroupId = id;

            EnlistUnits(enlistedUnits);

            commander = formation.AssignUnitPositions(enlistedUnits);
            commander.OnGroupOrder += OnGroupOrder;
            Debug.Log("assigned postions");
            this.formation = formation;
        }

        public UnitGroup(int id, Vector3 position, List<Unit> enlistedUnits, UnitFormation formation)
        {
            GroupId = id;

            EnlistUnits(enlistedUnits);

            commander = formation.AssignUnitPositions(enlistedUnits);
            commander.OnGroupOrder += OnGroupOrder;

            this.formation = formation;
            Debug.Log("assigned postions");
            OnGroupOrder(position);
        }

        public void RemoveUnit(Unit unit)
        {
            EnlistedUnits.Remove(unit);
        }

        private void EnlistUnits(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                unit.AssignGroupId(GroupId);
                EnlistedUnits.Add(unit);
            }
        }

        private void OnGroupOrder(Vector3 wayPoint)
        {
            formation.transform.position = wayPoint;
            formation.transform.rotation = Quaternion.LookRotation(commander.transform.position - wayPoint);
        }
    }
}