namespace BWolf.MutatableSystems
{
    /// <summary>
    /// To be implemented by an object that mutates a value of a <see cref="PointSystem"/>.
    /// </summary>
    public interface IPointMutator
    {
        /// <summary>
        /// Returns the array of names of points to be mutated.
        /// </summary>
        /// <returns>The names of the points to be mutated.</returns>
        string[] GetPointNames();
        
        /// <summary>
        /// The mutated value.
        /// </summary>
        /// <returns>The mutated value.</returns>
        int GetMutatedValue();
    }
}
