using BWolf.Utilities.PluggableStates;
using UnityEngine;

namespace BWolf.Examples.RealTimeStrategy
{
    [CreateAssetMenu(fileName = "AgentStopPickup", menuName = "AgentControl/Decisions/PickupStop")]
    public class PickupStopCondition : Condition
    {
        public override bool Check(StateController controller)
        {
            return true;
        }
    }
}