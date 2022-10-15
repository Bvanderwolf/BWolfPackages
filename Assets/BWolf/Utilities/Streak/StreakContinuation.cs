namespace BWolf.Utilities
{
    /// <summary>
    /// The result of a streak continuation.
    /// </summary>
    public enum StreakContinuation : int
    {
        /// <summary>
        /// The streak has reached its ceiling.
        /// </summary>
        REACHED_CEILING = 0,
        
        /// <summary>
        /// The streak is on cooldown still.
        /// </summary>
        ON_COOLDOWN = 1,
        
        /// <summary>
        /// The interval for the streak has been missed.
        /// </summary>
        MISSED_INTERVAL = 2,
        
        /// <summary>
        /// The streak was successfully incremented.
        /// </summary>
        SUCCESFULL = 3
    }
}
