namespace BWolf.Utilities
{
    /// <summary>
    /// The result of a streak continuation.
    /// </summary>
    public enum StreakContinuation
    {
        /// <summary>
        /// The streak has reached its ceiling.
        /// </summary>
        REACHED_CEILING,
        
        /// <summary>
        /// The streak is on cooldown still.
        /// </summary>
        ON_COOLDOWN,
        
        /// <summary>
        /// The interval for the streak has been missed.
        /// </summary>
        MISSED_INTERVAL,
        
        /// <summary>
        /// The streak was successfully incremented.
        /// </summary>
        SUCCESFULL
    }
}
