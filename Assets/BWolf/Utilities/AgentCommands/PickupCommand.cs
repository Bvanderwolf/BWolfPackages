namespace BWolf.Utilities.AgentCommands
{
    public class PickupCommand : CompositeCommand
    {
        public PickupCommand(Agent agent, PickupableObject pickupable) : base(new MoveCommand(agent, pickupable.GetPickupPosition(agent.transform.position)), new Command(agent, pickupable))
        {
        }

        /// <summary>subclass containing the agent command used to pickup the pickupable</summary>
        private class Command : AgentCommand
        {
            public PickupableObject Pickupable { get; private set; }

            public Command(Agent agent, PickupableObject pickupable) : base(agent)
            {
                Pickupable = pickupable;
            }

            public override void Excecute()
            {
                Pickupable.StartPickup();
                Agent.Controller.Transition("AgentStatePickup");
            }

            public override bool IsFinished()
            {
                return Pickupable.IsPickedUp();
            }

            public override void Undo()
            {
                Pickupable.UndoPickup();
                Agent.Controller.SetDefault();
            }
        }
    }
}