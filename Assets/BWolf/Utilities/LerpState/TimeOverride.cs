using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents different ways total time is changed
    /// when a new total time is set.
    /// </summary>
    public enum TimeOverride
    {
        /// <summary>
        /// Current time stays the same.
        /// </summary>
        [InspectorName("Keep current")]
        KEEP_CURRENT_TIME = 0,
        
        /// <summary>
        /// The current time is changed to keep the percentage the same.
        /// </summary>
        [InspectorName("Keep percentage")]
        KEEP_CURRENT_PERCENTAGE = 1,
        
        /// <summary>
        /// Resets the interpolation state.
        /// </summary>
        [InspectorName("Reset")]
        RESET = 2,
    }
}
