using System;
using UnityEngine;

namespace BWolf.PlayerStatistics
{
    /// <summary>
    /// An abstract representation of a player statistic.
    /// </summary>
    public abstract class PlayerStat : ScriptableObject
    {
        /// <summary>
        /// Fired when this statistics value has changed.
        /// </summary>
        public event Action ValueChanged;

        /// <summary>
        /// Should reset the statistic to its base value.
        /// This is used for refreshing the player stats after
        /// a modifier is added or removed.
        /// </summary>
        public abstract void ResetToBaseValue();

        /// <summary>
        /// Should be called by the implementing class when the
        /// value of the statistic has changed.
        /// </summary>
        protected void OnValueChanged() => ValueChanged?.Invoke();
    }
}