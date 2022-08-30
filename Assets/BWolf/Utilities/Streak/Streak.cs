using System;
using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents continuable streak operation data.
    /// </summary>
    [Serializable]
    public class Streak 
    {
        /// <summary>
        /// Whether the streak has a ceiling it can hit.
        /// </summary>
        public bool HasCeiling
        {
            get => ceiling != -1;
            set
            {
                // Ceiling doesn't need updating if its value already matched the boolean argument.
                if ((value && ceiling != -1) || (!value && ceiling == -1))
                    return;
                
                ceiling = value ? 1 : -1;
            }
        }

        /// <summary>
        /// Whether the streak has a cooldown after being reset.
        /// </summary>
        public bool HasCooldown => cooldown != 0.0f;

        /// <summary>
        /// Whether the streak has a required interval time between continuations.
        /// </summary>
        public bool HasInterval => interval != 0.0f;

        /// <summary>
        /// The current streak number.
        /// </summary>
        public int Current => _current;
        
        /// <summary>
        /// The ceiling for the streak.
        /// </summary>
        public int ceiling;

        /// <summary>
        /// The cooldown after being reset.
        /// </summary>
        public float cooldown;

        /// <summary>
        /// The required interval time between continuations.
        /// </summary>
        public float interval;

        /// <summary>
        /// The current streak number.
        /// </summary>
        private int _current;

        /// <summary>
        /// The last time a continuation of the streak has been done.
        /// </summary>
        private float _lastContinueTime;

        /// <summary>
        /// The last time a reset of the streak has been done.
        /// </summary>
        private float _lastResetTime;

        /// <summary>
        /// Resets the streak.
        /// </summary>
        public void Reset()
        {
            _current = 0;
            _lastResetTime = Time.time;
        }

        /// <summary>
        /// Continues the streak, incrementing the current streak number if successful.
        /// </summary>
        /// <returns>The streak continuation result.</returns>
        public StreakContinuation Continue()
        {
            if (_current == ceiling)
                return StreakContinuation.REACHED_CEILING;

            float time = Time.time;
            
            if (HasCooldown && _current == 0)
            {
                // If the streak has a cooldown and is reset, check whether the streak can start again.
                float timeSinceLastReset = time - _lastResetTime;
                if (timeSinceLastReset < cooldown)
                    return StreakContinuation.ON_COOLDOWN;
            }

            if (HasInterval)
            {
                // If the streak has an interval, check whether the streak can continue.
                float timeSinceLastContinue = time - _lastContinueTime;
                if (timeSinceLastContinue < interval)
                {
                    Reset();
                    return StreakContinuation.MISSED_INTERVAL;
                }
            }

            _current++;
            _lastContinueTime = time;
            
            return StreakContinuation.SUCCESFULL;
        }
    }
}
