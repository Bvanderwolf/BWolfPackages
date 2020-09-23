// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.Flocking.Context;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>The base class for each flocking behaviour to derive from to implement one function to calculate the move to be made</summary>
    public abstract class FlockBehaviour : ScriptableObject
    {
        public abstract Vector3 CalculateStep(FlockUnit unit, List<ContextItem> context, Flock flock);
    }
}