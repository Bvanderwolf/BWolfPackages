using System.Collections.Generic;
using BWolf.Utilities.Flocking.Context;
using BWolf.Utilities.SquadFormations.Units;
using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Flocking
{
    [CreateAssetMenu(fileName = "SquadAlignment", menuName = "SquadFlockingBehaviours/SquadAlignment")]
    public class SquadAlignmentBehaviour : SquadFlockBehaviour
    {
        public override Vector3 CalculateStep(Unit unit, List<ContextItem> context, UnitGroupHandler handler)
        {
            if (context.Count == 0)
            {
                //if there is not context return the unit's forward vector
                return unit.transform.forward;
            }

            //get average heading of context by adding them toghether and then dividing it by the ammount of context
            Vector3 move = Vector3.zero;
            foreach (ContextItem item in context.Filtered(1 << unit.gameObject.layer))
            {
                move += item.ContextTransform.forward;
            }
            move /= context.Count;

            return move.normalized;
        }
    }
}