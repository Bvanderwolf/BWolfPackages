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
        public static readonly EasingFunction noEase = value => value;

        /// <summary>
        /// A method that returns a value eased out using sine math.
        /// </summary>
        public static readonly EasingFunction easeOutSine = value => Mathf.Sin((value * Mathf.PI) / 2);

        /// <summary>
        /// A method that returns a value eased in using sine math.
        /// </summary>
        public static readonly EasingFunction easeInSine = value => 1 - Mathf.Cos((value * Mathf.PI) / 2);

        /// <summary>
        /// A method that returns a value eased in and out using sine math.
        /// </summary>
        public static readonly EasingFunction easeInOutSine = value => -(Mathf.Cos(Mathf.PI * value) - 1) / 2;
    }
}
