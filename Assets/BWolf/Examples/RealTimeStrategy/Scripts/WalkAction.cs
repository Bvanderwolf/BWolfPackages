using BWolf.Utilities.PluggableStates;
using UnityEngine;

namespace BWolf.Examples.RealTimeStrategy
{
    [CreateAssetMenu(fileName = "AgentWalk", menuName = "AgentControl/Actions/Walk")]
    public class WalkAction : Action
    {
        public override void Act(StateController controller)
        {
        }
    }
}