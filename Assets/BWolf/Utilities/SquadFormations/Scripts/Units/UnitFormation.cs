﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using System.Collections.Generic;
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
        private float minSpeed = 5f;

        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        private float speedCapAtDistance = 10f;

        [SerializeField]
        private float prioritySpeedMultiplier = 0.5f;

        [Header("Formation Creation")]
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

        public event Action<FormationSetting> OnFormationUpdate;

        public FormationSetting CurrentSetting { get; private set; }

        public List<FormationSetting> StoredSettings
        {
            get { return new List<FormationSetting>(storedSettings); }
        }

        private Vector3 targetPosition;
        private Vector3 startPosition;

        private Quaternion targetOrientation;
        private Quaternion startOrientation;

        private float translateDistance;
        private float translateSpeed;
        private float translateTime;
        private float currentTranslateTime;

        private void Awake()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetGizmo(gizmoColor, gizmoRadius);
            }

            if (storedSettings.Count != 0)
            {
                UpdateSetting(storedSettings[0].Name);
            }
            else
            {
                Debug.LogError("no formation settings have been configured! Make sure there is atleast one");
            }
        }

        private void OnValidate()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetGizmo(gizmoColor, gizmoRadius);
            }
        }

        private void FixedUpdate()
        {
            if (currentTranslateTime != translateTime)
            {
                //if current translate time hasn't reached translateTime it can be incremented by time since last frame
                currentTranslateTime += Time.deltaTime;
                if (currentTranslateTime >= translateTime)
                {
                    currentTranslateTime = translateTime;
                }

                float translateDelta = currentTranslateTime / translateTime;
                //update position and rotation based on translate delta if start and target are not the same and target hasn't been reached yet
                if (targetPosition != startPosition && transform.position != targetPosition)
                {
                    transform.position = Vector3.Lerp(startPosition, targetPosition, translateDelta);
                }

                if (targetOrientation != startOrientation && transform.rotation != targetOrientation)
                {
                    transform.rotation = Quaternion.Lerp(startOrientation, targetOrientation, translateDelta);
                }
            }
        }

        /// <summary>Sets the position and orientation target for this formation so it can linearly interpolate towards it</summary>
        public void SetTarget(Vector3 position)
        {
            targetPosition = position;
            startPosition = transform.position;

            translateDistance = (targetPosition - startPosition).magnitude;
            translateSpeed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Min(translateDistance, speedCapAtDistance) / speedCapAtDistance);
            translateTime = translateDistance / translateSpeed;

            targetOrientation = Quaternion.LookRotation(targetPosition - startPosition);
            startOrientation = transform.rotation;

            currentTranslateTime = 0;
        }

        /// <summary>creates a new formation positions adding it to the formation positions list aswell</summary>
        public void CreateFormationPosition()
        {
            FormationPosition position = Instantiate(prefabFormationPosition, transform).GetComponent<FormationPosition>();
            position.SetGizmo(gizmoColor, gizmoRadius);
            formationPositions.Add(position);
        }

        /// <summary>Clears all formation positions by destroying them and removing them from the formation positions list</summary>
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

        /// <summary>Creates a new formation setting using current value of the formation name</summary>
        public void CreateFormationSetting()
        {
            if (!string.IsNullOrEmpty(formationName) && !storedSettings.HasSettingWithName(formationName))
            {
                storedSettings.Add(new FormationSetting(formationName, formationPositions.Count, formationPositions.Points(true), gizmoColor, gizmoRadius));
            }
        }

        /// <summary>Removes formation setting based on current value of formation name</summary>
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

        /// <summary>Clears formation positions and stored settings</summary>
        public void ClearFormationSettings()
        {
            ClearFormationPositions();
            storedSettings.Clear();
            formationName = string.Empty;
        }

        /// <summary>Updates the formation positins to largest stored setting</summary>
        private void SetToLargestSetting()
        {
            UpdateFormationPositions(storedSettings.Largest());
        }

        /// <summary>Updates the formation positions with formation setting</summary>
        private void UpdateFormationPositions(FormationSetting setting)
        {
            ClearFormationPositions();

            formationName = setting.Name;
            gizmoColor = setting.GizmoColor;
            gizmoRadius = setting.GizmoRadius;

            for (int i = 0; i < setting.Size; i++)
            {
                FormationPosition position = Instantiate(prefabFormationPosition, transform).GetComponent<FormationPosition>();
                position.transform.localPosition = setting.LocalPositions[i];
                position.SetGizmo(gizmoColor, gizmoRadius);
                formationPositions.Add(position);
            }
        }

        /// <summary>Updates the current formation setting based on given name of new setting</summary>
        public void UpdateSetting(string nameOfSetting)
        {
            if (!string.IsNullOrEmpty(nameOfSetting) && CurrentSetting.Name != nameOfSetting)
            {
                //if the name of the new setting is not null, empty or the same as the current setting try finding it in the stored settings
                FormationSetting setting;
                if (storedSettings.GetSettingWithName(nameOfSetting, out setting))
                {
                    UpdateFormationPositions(setting);
                    OnFormationUpdate?.Invoke(setting);
                    CurrentSetting = setting;
                }
            }
        }

        /// <summary>Assigns given units a position in the formation, returning a commander for the group</summary>
        public Unit AssignUnits(List<Unit> units)
        {
            //make sure all positions in the formation are un assigned
            UnAssignPositions();

            //return commander based on assigned units to positions
            return AssignUnitsToPositions(units);
        }

        /// <summary>Unassigns this formations formation positions</summary>
        private void UnAssignPositions()
        {
            foreach (FormationPosition position in formationPositions)
            {
                position.SetAssigned(false);
            }
        }

        /// <summary>Assigns each unassigned formation position to a unit based on distance</summary>
        private Unit AssignUnitsToPositions(List<Unit> units)
        {
            Unit commander = null;
            for (int assignments = 0; assignments < units.Count; assignments++)
            {
                FormationPosition closestToCenter = ClosestToCenterOfFormation(formationPositions.UnAssigned());
                closestToCenter.SetAssigned(true);

                Unit closestUnit = ClosestUnitToFormationPosition(units.UnAssigned(), closestToCenter);
                closestUnit.AssignPosition(closestToCenter);

                float perc = assignments / (float)units.Count;
                closestUnit.AssignPrioritySpeed(minSpeed + ((translateSpeed * perc) * prioritySpeedMultiplier));

                if (assignments == 0)
                {
                    commander = closestUnit;
                }
            }

            return commander;
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