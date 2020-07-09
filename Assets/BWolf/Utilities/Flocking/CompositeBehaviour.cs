using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    [CreateAssetMenu(menuName = "Flocking/Composite")]
    public class CompositeBehaviour : FlockBehaviour
    {
        [SerializeField]
        private WeightedFlockBehaviour[] behaviours = null;

        public override Vector3 CalculateMove(FlockUnit unit, List<Transform> context, Flock flock)
        {
            if (behaviours.Length == 0)
            {
                //if no behaviours are set return a vector without any magnitude
                return Vector3.zero;
            }

            Vector3 move = Vector3.zero;
            foreach (var behaviour in behaviours)
            {
                Vector3 partialMove = behaviour.Behaviour.CalculateMove(unit, context, flock) * behaviour.Weight;
                if (partialMove.sqrMagnitude > behaviour.SqrWeight)
                {
                    partialMove = partialMove.normalized * behaviour.Weight;
                }

                move += partialMove;
            }

            return move;
        }

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