﻿using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Class for containg all information on the formation to be used by a unit group</summary>
    public class UnitFormation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GameObject prefabFormationPosition = null;

        [Header("Settings")]
        [SerializeField]
        private string formationName = null;

        [SerializeField]
        private Color gizmoColor = Color.red;

        [SerializeField]
        private float gizmoRadius = 1f;

        [SerializeField]
        private List<FormationPosition> formationPositions = null;

        [SerializeField]
        private List<FormationSetting> storedSettings = null;

        public FormationSetting CurrentSetting { get; private set; }

        private void Awake()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetGizmo(gizmoColor, gizmoRadius);
            }
        }

        private void OnValidate()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetGizmo(gizmoColor, gizmoRadius);
            }
        }

        public void AddFormationPosition()
        {
            FormationPosition position = Instantiate(prefabFormationPosition, transform).GetComponent<FormationPosition>();
            position.SetGizmo(gizmoColor, gizmoRadius);
            formationPositions.Add(position);
        }

        public void ClearFormationPositions()
        {
            for (int i = formationPositions.Count - 1; i >= 0; i--)
            {
                if (formationPositions[i] != null)
                {
                    DestroyImmediate(formationPositions[i].gameObject);
                }
                formationPositions.RemoveAt(i);
            }
        }

        public void CreateFormationSetting()
        {
            if (!string.IsNullOrEmpty(formationName) && !storedSettings.HasSettingWithName(formationName))
            {
                storedSettings.Add(new FormationSetting(formationName, formationPositions.Count, formationPositions.Points(true), gizmoColor, gizmoRadius));
            }
        }

        public void RemoveFormationSetting()
        {
            if (storedSettings.HasSettingWithName(formationName))
            {
                storedSettings.RemoveSetttingWithName(formationName);
                if (storedSettings.Count != 0)
                {
                    SetToLargestSetting();
                }
                else
                {
                    ClearFormationSettings();
                }
            }
        }

        public void ClearFormationSettings()
        {
            ClearFormationPositions();
            storedSettings.Clear();
            formationName = string.Empty;
        }

        private void SetToLargestSetting()
        {
            ClearFormationPositions();

            FormationSetting largest = storedSettings.Largest();
            formationName = largest.Name;
            gizmoColor = largest.GizmoColor;
            gizmoRadius = largest.GizmoRadius;

            for (int i = 0; i < largest.Size; i++)
            {
                FormationPosition position = Instantiate(prefabFormationPosition, largest.LocalPositions[i], Quaternion.identity, transform).GetComponent<FormationPosition>();
                position.SetGizmo(gizmoColor, gizmoRadius);
                formationPositions.Add(position);
            }
        }

        public void UpdateSetting(string nameOfSetting)
        {
            FormationSetting setting;
            if (storedSettings.GetSettingWithName(nameOfSetting, out setting))
            {
                CurrentSetting = setting;
            }
        }

        /// <summary>Assigns given units a position in the formation, returning a commander for the group</summary>
        public Unit AssignUnitPositions(List<Unit> units)
        {
            //make sure all positions in the formation are un assigned
            UnAssign();

            //assign each unassigned formation position a unit based on distance
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

        /// <summary>Unassigns this formations formation positions</summary>
        private void UnAssign()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetAssigned(false);
            }
        }

        /// <summary>Returns the closest formation position in given list to the center of the formation</summary>
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

        /// <summary>Returns the unit in given unit list that is closest to the given formation position</summary>
        private Unit ClosestUnitToFormationPosition(List<Unit> units, FormationPosition position)
        {
            float closestSqrMagnitude = float.MaxValue;
            Unit closest = null;
            foreach (Unit unit in units)
            {
                float sqrmagnitude = (unit.transform.position - position.Point(false)).sqrMagnitude;
                if (sqrmagnitude < closestSqrMagnitude)
                {
                    closest = unit;
                    closestSqrMagnitude = sqrmagnitude;
                }
            }

            return closest;
        }

        /// <summary>Returns the center of all formation positions toghether</summary>
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