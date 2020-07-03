namespace BWolf.Examples.SquadFormations.Interactions
{
    public struct Interaction
    {
        public readonly InteractionType TypeOfInteraction;
        public readonly object InteractionContent;

        public Interaction(InteractionType interactionType, object content)
        {
            TypeOfInteraction = interactionType;
            InteractionContent = content;
        }
    }
}