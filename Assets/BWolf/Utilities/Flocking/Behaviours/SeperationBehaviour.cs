using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>One of 3 main behaviours for flocking, making sure units don't collide with each other</summary>
    [CreateAssetMenu(fileName = "SeperationBehaviour", menuName = "FlockingBehaviours/Seperation")]
    public class SeperationBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateStep(FlockUnit unit, List<ContextItem> context, Flock flock)
        {
            if (context.Count == 0)
            {
                //if there is no context return a vector with no magnitude
                return Vector3.zero;
            }

            //check for each context item if it is to be avoided and add direction away from it to the move if need be
            Vector3 move = Vector3.zero;
            int avoidNr = 0;
            foreach (ContextItem item in context.Filtered(1 << unit.gameObject.layer))
            {
                Vector3 position = item.ContextTransform.position;
                if ((position - unit.transform.position).sqrMagnitude < flock.SqrAvoidanceRadius)
                {
                    avoidNr++;
                    move += (unit.transform.position - position);
                }
            }

            if (avoidNr > 0)
            {
                //if there where items to be avoided, divide the move vector by the number of items
                move /= avoidNr;
            }

            return move;
        }
    }
}