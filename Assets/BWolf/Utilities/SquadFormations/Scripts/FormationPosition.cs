using UnityEngine;

namespace BWolf.Utilities.SquadFormations
{
    public class FormationPosition : MonoBehaviour
    {
        private Color gizmoColor;
        private float gizmoRadius;

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