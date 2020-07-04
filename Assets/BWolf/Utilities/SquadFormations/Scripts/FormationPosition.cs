using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class FormationPosition : MonoBehaviour
    {
        private Color gizmoColor;
        private float gizmoRadius;

        public bool Assigned { get; private set; }

        public Vector3 Point
        {
            get { return transform.position; }
        }

        public void SetAssigned(bool value)
        {
            Assigned = value;
        }

        public void SetGizmo(Color color, float radius)
        {
            gizmoColor = color;
            gizmoRadius = radius;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }
    }
}