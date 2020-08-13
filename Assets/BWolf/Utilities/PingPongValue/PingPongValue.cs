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

        public float value
        {
            get
            {
                return positiveMin ? min + Mathf.PingPong(currentTime, max) : min + Mathf.PingPong(currentTime, Mathf.Abs(min) + Mathf.Abs(max));
            }
        }

        public PingPongValue(float min, float max, int count, float startPerc = 0)
        {
            this.min = min;
            this.max = max;

            positiveMin = min >= 0;

            float diff = Mathf.Abs(min) + Mathf.Abs(max);
            currentTime = startPerc * diff;
            time = ((count * 2f) * diff) + currentTime;
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