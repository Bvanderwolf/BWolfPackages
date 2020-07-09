using BWolf.Utilities.Flocking;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>Custom flocking behaviour for bounding units inside given bounds, making sure they stay inside</summary>
    [CreateAssetMenu(fileName = "BoundedBehaviour", menuName = "FlockingBehaviours/Bounded")]
    public class BoundedBehaviour : FlockBehaviour
    {
        [SerializeField]
        private Bounds bounds;

        public override Vector3 CalculateStep(FlockUnit unit, List<ContextItem> context, Flock flock)
        {
            if (!bounds.Contains(unit.transform.position))
            {
                //if the unit is outside the bounds, return a move that is towards the closest point on the bounds from the units position
                Vector3 position = unit.transform.position;
                return flock.FlockBounds.ClosestPoint(position) - position;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}