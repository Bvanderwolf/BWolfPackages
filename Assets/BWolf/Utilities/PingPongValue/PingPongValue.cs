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

        private bool positiveMin;
        private float minMaxDifference;

        public float value
        {
            get
            {
                return positiveMin ? min + Mathf.PingPong(currentTime, max) : min + Mathf.PingPong(currentTime, minMaxDifference);
            }
        }

        public PingPongValue(float min, float max, int count, float startPerc = 0)
        {
            this.min = min;
            this.max = max;

            positiveMin = min >= 0;

            minMaxDifference = Mathf.Abs(min) + Mathf.Abs(max);
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