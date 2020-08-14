// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities
{
    public struct PingPongValue
    {
        private float min;
        private float max;

        private float currentTime;
        private float time;

        private float minMaxDifference;

        public float value
        {
            get
            {
                return min + Mathf.PingPong(currentTime, minMaxDifference);
            }
        }

        /// <summary>Creates a new pingpong value object with a mininum value and a max value, a pingpong count and an optional
        /// start perc at which the value should start</summary>
        public PingPongValue(float min, float max, int count, float startPerc = 0)
        {
            if (min > max)
            {
                throw new System.Exception($"min: {min} is greater than max: {max} :: this is not intended!");
            }

            this.min = min;
            this.max = max;

            if (min < 0 && max > 0)
            {
                minMaxDifference = Mathf.Abs(min - max);
            }
            else
            {
                minMaxDifference = max - min;
            }

            currentTime = startPerc * minMaxDifference;
            time = ((count * 2f) * minMaxDifference) + currentTime;
        }

        /// <summary>
        /// Returns if set count hasn't been reached. Increments current time by delta if this is the case
        /// </summary>
        public bool Continue(float delta)
        {
            if (currentTime == time)
            {
                return false;
            }

            currentTime += delta;
            if (currentTime > time)
            {
                currentTime = time;
            }
            return true;
        }
    }
}