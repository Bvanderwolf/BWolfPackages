namespace BWolf.Utilities
{
    /// <summary>
    /// The abstract representation of linear interpolation state.
    /// </summary>
    public abstract class LerpState : IContinuable
    {
        /// <summary>
        /// The total time that needs to pass before the interpolation is finished.
        /// </summary>
        public abstract float TotalTime { get; }
        
        /// <summary>
        /// The current percentage of linear interpolation.
        /// </summary>
        public abstract float Percentage { get; }
        
        /// <summary>
        /// The remaining time to linearly interpolate.
        /// </summary>
        public abstract float RemainingTime { get; }
        
        /// <summary>
        /// Resets the linear interpolation state.
        /// </summary>
        public abstract void Reset();
        
        /// <summary>
        /// Continues the linear interpolation.
        /// </summary>
        /// <returns>Whether it succeeded.</returns>
        public abstract bool Continue();
    }
}
