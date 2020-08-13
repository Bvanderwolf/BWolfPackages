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
                return positiveMin ? Mathf.PingPong(currentTime, max) : min + Mathf.PingPong(currentTime, Mathf.Abs(min - max));
            }
        }

        public PingPongValue(float min, float max, int count, float startPerc = 0)
        {
            this.min = min;
            this.max = max;

            positiveMin = min >= 0;

            float diff = Mathf.Abs(min - max);
            currentTime = startPerc * diff;
            time = ((count * 2f) * diff) + currentTime;
        }

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