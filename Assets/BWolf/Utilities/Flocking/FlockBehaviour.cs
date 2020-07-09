using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public abstract class FlockBehaviour : ScriptableObject
    {
        public abstract Vector3 CalculateMove(FlockUnit unit, List<Transform> context, Flock flock);
    }
}