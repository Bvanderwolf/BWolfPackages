using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Units
{
    /// <summary>Component class representing a position in a unit formation</summary>
    public class FormationPosition : MonoBehaviour
    {
        public bool Assigned { get; private set; }
        public Vector3 LookPosition { get; private set; }

        private const float directionLineLength = 1.5f;

        private Color gizmoColor;
        private float gizmoRadius;

        /// <summary>Returns position in local or worldspace of the formation position</summary>
        public Vector3 Point(bool local)
        {
            return local ? transform.localPosition : transform.position;
        }

        /// <summary>Sets whether this formation position has been assigned to a unit or not</summary>
        public void SetAssigned(bool value)
        {
            Assigned = value;
        }

        /// <summary>Sets the gizmo color and radius of this formation position</summary>
        public void SetGizmo(Color color, float radius)
        {
            gizmoColor = color;
            gizmoRadius = radius;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);

            Vector3 position = transform.position;
            LookPosition = position + (transform.forward * directionLineLength);
            Gizmos.DrawLine(position, LookPosition);
        }
    }
}