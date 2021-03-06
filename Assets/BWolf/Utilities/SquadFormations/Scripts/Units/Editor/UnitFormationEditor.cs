﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEditor;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Editor script for creation of new unit formations using the Unit Formation MonoBehaviour</summary>
    [CustomEditor(typeof(UnitFormation))]
    public class UnitFormationEditor : Editor
    {
        private const float pixelSpace = 10;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(pixelSpace);

            GUILayout.Label("Formation Management", EditorStyles.boldLabel);

            UnitFormation formation = (UnitFormation)target;
            Undo.RecordObject(formation, "change children");
            PrefabUtility.RecordPrefabInstancePropertyModifications(formation.gameObject);

            if (GUILayout.Button(new GUIContent("Add FormationPosition", "Creates Formation position and adds it to formation positions list")))
            {
                formation.CreateFormationPosition();
            }

            GUILayout.Space(pixelSpace * 0.5f);

            if (GUILayout.Button(new GUIContent("Clear FormationPositions", "Clears formation positions without deleting the setting")))
            {
                formation.ClearFormationPositions();
            }

            GUILayout.Space(pixelSpace);

            if (GUILayout.Button(new GUIContent("Store Setting", "Saves Formation Setting using current values")))
            {
                formation.CreateFormationSetting();
            }

            GUILayout.Space(pixelSpace * 0.5f);

            if (GUILayout.Button(new GUIContent("Remove Setting", "Removes Formation Setting using current name value")))
            {
                formation.RemoveFormationSetting();
            }

            GUILayout.Space(pixelSpace * 0.5f);

            if (GUILayout.Button(new GUIContent("Clear Settings", "Clears all stored settings")))
            {
                formation.ClearFormationSettings();
            }
        }
    }
}