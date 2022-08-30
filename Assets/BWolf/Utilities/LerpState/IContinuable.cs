namespace BWolf.Utilities
{
    /// <summary>
    /// To be implemented by a continuable stateful object.
    /// </summary>
    public interface IContinuable
    {
        /// <summary>
        /// Continues the state, returning whether it succeeded.
        /// </summary>
        /// <returns>Whether it succeeded at continuing.</returns>
        bool Continue();
    }
}
