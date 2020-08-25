using UnityEngine;

namespace BWolf.Utilities.AgentCommands
{
    [CreateAssetMenu(fileName = "AgentStopPickup", menuName = "AgentControl/Decisions/PickupStop")]
    public class PickupStopCondition : Condition
    {
        public override bool Check(StateController controller)
        {
            return controller.CurrentCommand as PickupCommand != null;
        }
    }
}