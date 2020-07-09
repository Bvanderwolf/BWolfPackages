using BWolf.Utilities.Flocking;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.Flocking
{
    [CreateAssetMenu(menuName = "Flocking/TerrainBound")]
    public class TerrainBoundBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateMove(FlockUnit unit, List<Transform> context, Flock flock)
        {
            if (!flock.FlockBounds.Contains(unit.transform.position))
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