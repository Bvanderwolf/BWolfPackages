using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
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