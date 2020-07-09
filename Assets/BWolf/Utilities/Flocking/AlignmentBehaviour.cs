using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    [CreateAssetMenu(menuName = "Flocking/Alignment")]
    public class AlignmentBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateMove(FlockUnit unit, List<Transform> context, Flock flock)
        {
            if (context.Count == 0)
            {
                //if there is not context return the unit's forward vector
                return unit.transform.forward;
            }

            //get average heading of context by adding them toghether and then dividing it by the ammount of context
            Vector3 move = Vector3.zero;
            foreach (Transform t in context)
            {
                move += t.forward;
            }
            move /= context.Count;

            return move.normalized;
        }
    }
}