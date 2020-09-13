// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>structure representing a setting a formation can be in</summary>
    [System.Serializable]
    public struct FormationSetting
    {
        public string Name;
        public Color GizmoColor;
        public float GizmoRadius;
        public int Size;
        public Vector3[] LocalPositions;

        public FormationSetting(string name, int size, Vector3[] localPositions, Color gizmoColor, float gizmoRadius)
        {
            Name = name;
            Size = size;
            LocalPositions = localPositions;
            GizmoColor = gizmoColor;
            GizmoRadius = gizmoRadius;
        }
    }
}