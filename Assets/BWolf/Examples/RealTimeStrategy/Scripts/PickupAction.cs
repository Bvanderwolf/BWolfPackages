using BWolf.Utilities.PluggableStates;
using UnityEngine;

namespace BWolf.Examples.RealTimeStrategy
{
    [CreateAssetMenu(fileName = "AgentPickup", menuName = "AgentControl/Actions/Pickup")]
    public class PickupAction : Action
    {
        public override void Act(StateController controller)
        {
        }
    }
}