namespace BWolf.Utilities.ShapeShifting
{
    /// <summary>
    /// Interface to be implemented by objects that need templatability
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITemplatable<T>
    {
        T GetTemplate();
    }
}