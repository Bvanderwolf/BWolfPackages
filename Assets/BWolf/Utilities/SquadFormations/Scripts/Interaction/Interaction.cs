namespace BWolf.Utilities.SquadFormations.Interactions
{
    /// <summary>structure for describing an interaction containg a type and content matching it</summary>
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