﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.Flocking.Context;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>1 of the 3 main behaviours for flocking, making sure units move towards the same direction</summary>
    [CreateAssetMenu(fileName = "AlignmentBehaviour", menuName = "FlockingBehaviours/Alignment")]
    public class AlignmentBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateStep(FlockUnit unit, List<ContextItem> context, Flock flock)
        {
            if (context.Count == 0)
            {
                //if there is not context return the unit's forward vector
                return unit.transform.forward;
            }

            //get average heading of context by adding them toghether and then dividing it by the ammount of context
            Vector3 move = Vector3.zero;
            foreach (ContextItem item in context.FilteredToLayer(1 << unit.gameObject.layer))
            {
                move += item.ContextTransform.forward;
            }
            move /= context.Count;

            return move.normalized;
        }
    }
}