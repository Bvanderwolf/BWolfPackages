// Easing functions gotten from https://easings.net/#easeInOutSine
//----------------------------------------------------------------

using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Defines usable easing functions.
    /// </summary>
    public static class EasingFunctions
    {
        /// <summary>
        /// A method that does no easing.
        /// </summary>
        public static readonly EasingFunction noEase = (c, t) => c / t;

        /// <summary>
        /// A method that returns a value eased out using sine math.
        /// </summary>
        public static readonly EasingFunction easeOutSine = (c, t) => Mathf.Sin( c * Mathf.PI * 0.5f) / t;

        /// <summary>
        /// A method that returns a value eased in using sine math.
        /// </summary>
        public static readonly EasingFunction easeInSine = (c, t) =>  (1f - Mathf.Cos( c * Mathf.PI * 0.5f)) / t;

        /// <summary>
        /// A method that returns a value eased in and out using sine math.
        /// </summary>
        public static readonly EasingFunction easeInOutSine = (c, t) => (-(Mathf.Cos(Mathf.PI * c) - 1) * 0.5f) / t;
    }
}
