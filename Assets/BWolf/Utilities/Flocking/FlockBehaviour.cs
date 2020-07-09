using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public abstract class FlockBehaviour : ScriptableObject
    {
        public abstract void CalculateMove(FlockUnit agent, List<Transform> context, Flock flock);
    }
}