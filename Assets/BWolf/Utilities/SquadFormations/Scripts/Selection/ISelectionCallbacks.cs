namespace BWolf.Utilities.SquadFormations.Selection
{
    public interface ISelectionCallbacks
    {
        void OnSelect();

        void OnDeselect();

        void OnHoverStart();

        void OnHoverEnd();
    }
}