using System.Collections.Generic;
using System.Linq;
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

        public List<FormationPosition> FormationPositions
        {
            get { return formationPositions; }
        }

        public FormationState State { get; private set; }

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

        public FormationPosition CenterPosition
        {
            get
            {
                Vector3 center = GetCenterOfPositions();
                float closestSqrMagnitude = float.MaxValue;
                FormationPosition closest = null;
                foreach (FormationPosition formationPosition in formationPositions)
                {
                    if ((formationPosition.transform.position - center).sqrMagnitude < closestSqrMagnitude)
                    {
                        closest = formationPosition;
                    }
                }

                return closest;
            }
        }

        public Vector3[] Positions
        {
            get { return formationPositions.Select(f => f.transform.position).ToArray(); }
        }

        private Vector3 GetCenterOfPositions()
        {
            Vector3 center = Vector3.zero;
            foreach (FormationPosition formationPosition in formationPositions)
            {
                center += formationPosition.transform.position;
            }
            return center / formationPositions.Count;
        }

        public enum FormationState
        {
            Broken,
            Forming,
            Formed
        }
    }
}