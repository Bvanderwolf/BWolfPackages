using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>1 of the 3 main behaviours for flocking, making sure units are moving towards the group</summary>
    [CreateAssetMenu(fileName = "CohesionBehaviour", menuName = "FlockingBehaviours/Cohesion")]
    public class CohesionBehaviour : FlockBehaviour
    {
        public override Vector3 CalculateMove(FlockUnit unit, List<ContextItem> context, Flock flock)
        {
            if (context.Count == 0)
            {
                //if there is not context return a vector with no magnitude
                return Vector3.zero;
            }

            //get average position of context by adding them toghether and then dividing it by the ammount of context
            Vector3 move = Vector3.zero;
            foreach (ContextItem item in context.Filtered(1 << unit.gameObject.layer))
            {
                move += item.ContextTransform.position;
            }
            move /= context.Count;

            //subtract units position to make the move the direction towards the average position
            move -= unit.transform.position;

            //damp move to prefent flickering
            move = Vector3.SmoothDamp(unit.transform.forward, move, ref flock.Velocity, 0.5f);

            return move;
        }
    }
}