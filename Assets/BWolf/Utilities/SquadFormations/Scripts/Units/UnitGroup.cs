using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>class for representing a group of units</summary>
    public class UnitGroup
    {
        public readonly List<Unit> EnlistedUnits = new List<Unit>();
        public readonly int GroupId;

        public Unit Commander { get; private set; }
        public UnitFormation Formation { get; private set; }

        /// <summary>Creates a group with id and a formation</summary>
        public UnitGroup(int id, UnitFormation formation)
        {
            GroupId = id;

            this.Formation = formation;
        }

        /// <summary>Assigns units to a group</summary>
        public List<Unit> AssignUnitsToGroup(List<Unit> units)
        {
            List<Unit> enlistableUnits = TrimToFitGroup(units);
            EnlistUnits(units);

            Commander = Formation.AssignUnitPositions(units);
            Commander.OnGroupOrder += OnGroupOrder;

            return enlistableUnits;
        }

        /// <summary>Assigns units to a group also giving them a order to move their formation towards given formation position</summary>
        public List<Unit> AssignUnitsToGroup(List<Unit> units, Vector3 formationPosition)
        {
            List<Unit> enlistableUnits = TrimToFitGroup(units);
            EnlistUnits(units);

            Commander = Formation.AssignUnitPositions(units);
            Commander.OnGroupOrder += OnGroupOrder;

            OnGroupOrder(formationPosition);

            return enlistableUnits;
        }

        /// <summary>Removes given unit from the enlisted units  in this group</summary>
        public void RemoveUnit(Unit unit)
        {
            unit.ResetValues();
            EnlistedUnits.Remove(unit);
        }

        /// <summary>Tries removing the last unit in the group if there is only one, returns whether it was succesfull or not</summary>
        public bool TryTrimLastUnit()
        {
            if (EnlistedUnits.Count == 1)
            {
                RemoveUnit(EnlistedUnits[0]);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>ReAssigns enlisted units to formation positions</summary>
        public void ReAssignUnits()
        {
            //set asigned position to null
            foreach (Unit unit in EnlistedUnits)
            {
                unit.AssignPosition(null);
            }

            //assign units receiving a new commander to call group orders on
            Commander = Formation.AssignUnitPositions(EnlistedUnits);
            Commander.OnGroupOrder += OnGroupOrder;
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
            ReAssignUnits();

            Formation.transform.position = formationWayPoint;
            Formation.transform.rotation = Quaternion.LookRotation(Commander.transform.position - formationWayPoint);
        }

        private List<Unit> TrimToFitGroup(List<Unit> units)
        {
            int trimCount = Mathf.Max(0, units.Count - Formation.CurrentSetting.Size);
            for (int i = 0; i < trimCount; i++)
            {
                units.RemoveAt(units.Count - 1);
            }
            return units;
        }
    }
}