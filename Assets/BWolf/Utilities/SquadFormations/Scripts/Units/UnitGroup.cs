using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>class for representing a group of units</summary>
    public class UnitGroup
    {
        public readonly List<Unit> EnlistedUnits = new List<Unit>();
        public readonly int GroupId;

        private Unit commander;
        private UnitFormation formation;

        /// <summary>Creates a group with id and a formation</summary>
        public UnitGroup(int id, UnitFormation formation)
        {
            GroupId = id;

            this.formation = formation;
        }

        /// <summary>Assigns units to a group</summary>
        public void AssignUnitsToGroup(List<Unit> units)
        {
            EnlistUnits(units);

            commander = formation.AssignUnitPositions(units);
            commander.OnGroupOrder += OnGroupOrder;
        }

        /// <summary>Assigns units to a group also giving them a order to move their formation towards given formation position</summary>
        public void AssignUnitsToGroup(List<Unit> units, Vector3 formationPosition)
        {
            EnlistUnits(units);

            commander = formation.AssignUnitPositions(units);
            commander.OnGroupOrder += OnGroupOrder;

            OnGroupOrder(formationPosition);
        }

        /// <summary>Removes given unit from the enlisted units  in this group</summary>
        public void RemoveUnit(Unit unit)
        {
            EnlistedUnits.Remove(unit);
        }

        /// <summary>Enlists given units in this group</summary>
        private void EnlistUnits(List<Unit> units)
        {
            foreach (Unit unit in units)
            {
                unit.AssignGroupId(GroupId);
                EnlistedUnits.Add(unit);
            }
        }

        /// <summary>Called when a group order is given to the commander to move the formation to a new waypoint</summary>
        private void OnGroupOrder(Vector3 formationWayPoint)
        {
            formation.transform.position = formationWayPoint;
            formation.transform.rotation = Quaternion.LookRotation(commander.transform.position - formationWayPoint);
        }
    }
}