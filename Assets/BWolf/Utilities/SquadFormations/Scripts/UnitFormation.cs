using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class UnitFormation : MonoBehaviour
    {
        [SerializeField]
        private Color gizmoColor = Color.red;

        [SerializeField]
        private float gizmoRadius = 1f;

        [SerializeField]
        private List<FormationPosition> formationPositions = null;

        private void OnValidate()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position?.SetGizmo(gizmoColor, gizmoRadius);
            }
        }

        private void Awake()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetGizmo(gizmoColor, gizmoRadius);
            }
        }

        public Unit AssignUnitPositions(List<Unit> units)
        {
            Unit commander = null;
            for (int assignments = 0; assignments < units.Count; assignments++)
            {
                FormationPosition closestToCenter = ClosestToCenterOfFormation(formationPositions.UnAssigned());
                closestToCenter.SetAssigned(true);

                Unit closestUnit = ClosestUnitToFormationPosition(units.UnAssigned(), closestToCenter);
                closestUnit.AssignPosition(closestToCenter);

                if (assignments == 0)
                {
                    commander = closestUnit;
                }
            }

            return commander;
        }

        private FormationPosition ClosestToCenterOfFormation(List<FormationPosition> positions)
        {
            Vector3 center = GetCenterOfFormation();
            float closestSqrMagnitude = float.MaxValue;
            FormationPosition closest = null;
            foreach (FormationPosition position in positions)
            {
                float sqrmagnitude = (position.transform.position - center).sqrMagnitude;
                if (sqrmagnitude < closestSqrMagnitude)
                {
                    closest = position;
                    closestSqrMagnitude = sqrmagnitude;
                }
            }

            return closest;
        }

        private Unit ClosestUnitToFormationPosition(List<Unit> units, FormationPosition position)
        {
            float closestSqrMagnitude = float.MaxValue;
            Unit closest = null;
            foreach (Unit unit in units)
            {
                float sqrmagnitude = (unit.transform.position - position.Point).sqrMagnitude;
                if (sqrmagnitude < closestSqrMagnitude)
                {
                    closest = unit;
                    closestSqrMagnitude = sqrmagnitude;
                }
            }

            return closest;
        }

        private Vector3 GetCenterOfFormation()
        {
            Vector3 center = Vector3.zero;
            foreach (FormationPosition formationPosition in formationPositions)
            {
                center += formationPosition.transform.position;
            }
            return center / formationPositions.Count;
        }
    }
}