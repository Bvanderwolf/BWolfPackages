using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    [CreateAssetMenu(menuName = "Flocking/Cohesion")]
    public class CohesionBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateMove(FlockUnit unit, List<Transform> context, Flock flock)
        {
            if (context.Count == 0)
            {
                //if there is not context return a vector with no magnitude
                return Vector3.zero;
            }

            //get average position of context by adding them toghether and then dividing it by the ammount of context
            Vector3 move = Vector3.zero;
            foreach (Transform t in context)
            {
                move += t.position;
            }
            move /= context.Count;

            //subtract units position to make the move the direction towards the average position
            move -= unit.transform.position;

            move = Vector3.SmoothDamp(unit.transform.forward, move, ref flock.Velocity, 0.5f);

            return move;
        }
    }
}