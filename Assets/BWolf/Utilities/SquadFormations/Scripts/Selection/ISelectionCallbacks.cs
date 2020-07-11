namespace BWolf.Utilities.SquadFormations.Selection
{
    /// <summary>Interface to be used for interfacing with the SelectableObject component (e.g. decal projection)</summary>
    public interface ISelectionCallbacks
    {
        void OnSelect();

        void OnDeselect();

        void OnHoverStart();

        void OnHoverEnd();
    }
}