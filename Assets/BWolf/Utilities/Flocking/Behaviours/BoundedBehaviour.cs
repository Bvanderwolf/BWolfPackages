using BWolf.Utilities.Flocking;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.Flocking
{
    [CreateAssetMenu(menuName = "Flocking/Bounded")]
    public class BoundedBehaviour : FlockBehaviour
    {
        [SerializeField]
        private Bounds bounds;

        public override Vector3 CalculateMove(FlockUnit unit, List<FlockUnitContext> context, Flock flock)
        {
            if (!bounds.Contains(unit.transform.position))
            {
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