using UnityEngine;

namespace BWolf.Utilities
{

    public enum TimeOverride
    {
        [InspectorName("Keep current")]
        KEEP_CURRENT_TIME = 0,
        
        [InspectorName("Keep percentage")]
        KEEP_CURRENT_PERCENTAGE = 1,
        
        [InspectorName("Reset")]
        RESET = 2,
    }
}
