using System.Collections.Generic;
using BWolf.Utilities.Flocking.Context;
using BWolf.Utilities.SquadFormations.Units;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Flocking
{
    [CreateAssetMenu(fileName = "SquadCompositeBehaviour", menuName = "SquadFlockingBehaviours/Composite")]
    public class SquadCompositeBehaviour : SquadFlockBehaviour
    {
        [SerializeField]
        private WeightedSquadFlockBehaviour[] behaviours = null;

        public override Vector3 CalculateStep(Unit unit, List<ContextItem> context, UnitGroupHandler handler)
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
                Vector3 partialMove = behaviour.Behaviour.CalculateStep(unit, context, handler) * behaviour.Weight;
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
        private struct WeightedSquadFlockBehaviour
        {
#pragma warning disable 0649
            public float Weight;
            public SquadFlockBehaviour Behaviour;
#pragma warning restore 0649

            public float SqrWeight { get { return Weight * Weight; } }
        }
    }
}