using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking.Behaviours
{
    /// <summary>The behaviour combining multiple flocking behaviours to create create the actuall flocking mechanic</summary>
    [CreateAssetMenu(fileName = "CompositeBehaviour", menuName = "FlockingBehaviours/Composite")]
    public class CompositeBehaviour : FlockBehaviour
    {
        [SerializeField]
        private WeightedFlockBehaviour[] behaviours = null;

        public override Vector3 CalculateStep(FlockUnit unit, List<ContextItem> context, Flock flock)
        {
            if (behaviours.Length == 0)
            {
                //if no behaviours are set return a vector without any magnitude
                return Vector3.zero;
            }

            Vector3 move = Vector3.zero;
            foreach (var behaviour in behaviours)
            {
                //add clamped partial moves to the to be returned move
                Vector3 partialMove = behaviour.Behaviour.CalculateStep(unit, context, flock) * behaviour.Weight;
                if (partialMove.sqrMagnitude > behaviour.SqrWeight)
                {
                    partialMove = partialMove.normalized * behaviour.Weight;
                }

                move += partialMove;
            }

            return move;
        }

        /// <summary>structure that makes it possible to add weights to each behaviour to be used</summary>
        [System.Serializable]
        private struct WeightedFlockBehaviour
        {
#pragma warning disable 0649
            public float Weight;
            public FlockBehaviour Behaviour;
#pragma warning restore 0649

            public float SqrWeight { get { return Weight * Weight; } }
        }
    }
}