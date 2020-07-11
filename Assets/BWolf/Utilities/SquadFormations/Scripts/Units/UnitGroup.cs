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
            this.Formation.OnFormationUpdate += OnFormationUpdate;
        }

        /// <summary>Assigns units to a group also giving them a order to move their formation towards given formation position</summary>
        public List<Unit> AssignUnitsToGroup(List<Unit> units, Vector3 formationPosition)
        {
            EnlistUnits(units);
            TrimUnitsToFitFormationSetting(Formation.CurrentSetting);

            SetFormationTarget(formationPosition);
            AssignFormationPositions();

            return EnlistedUnits;
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

            AssignFormationPositions();
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

        /// <summary>Assigns currently enlisted units as formation position and sets up the formation commander</summary>
        private void AssignFormationPositions()
        {
            //assign units receiving a commander to call group orders on
            Commander = Formation.AssignUnits(EnlistedUnits);
            Commander.OnGroupOrder += OnGroupOrder;

            Formation.transform.position = Commander.transform.position;
        }

        /// <summary>Called when the formation has been updated, it makes sure units move towards the new formation positions</summary>
        private void OnFormationUpdate(FormationSetting newSetting)
        {
            TrimUnitsToFitFormationSetting(newSetting);
            ReAssignUnits();
            MoveUnitsInFormation();
        }

        /// <summary>Gives each enlisted unit the order to move towards its assigned position</summary>
        private void MoveUnitsInFormation()
        {
            foreach (Unit unit in EnlistedUnits)
            {
                unit.MoveTowardsAssignedPosition();
            }
        }

        /// <summary>Called when a group order is given to the commander to move the formation to a new waypoint</summary>
        private void OnGroupOrder(Vector3 formationWayPoint)
        {
            SetFormationTarget(formationWayPoint);
        }

        /// <summary>Moves formwation towards given waypoint rotating it in the direction the units have to walk </summary>
        private void SetFormationTarget(Vector3 waypoint)
        {
            Formation.SetTarget(waypoint);
        }

        /// <summary>Trims given list of units to return a list that fits the group's formation size</summary>
        private void TrimUnitsToFitFormationSetting(FormationSetting setting)
        {
            int trimCount = Mathf.Max(0, EnlistedUnits.Count - setting.Size);
            for (int i = 0; i < trimCount; i++)
            {
                EnlistedUnits.RemoveAt(EnlistedUnits.Count - 1);
            }
        }
    }
}