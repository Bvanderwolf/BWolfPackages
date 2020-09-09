// Created By: Benjamin van der Wolf
// Version: 1.2
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities
{
    public struct PingPongValue
    {
        private float _min;
        private float _max;

        private float currentTime;
        private float _speed;

        private float minMaxDifference;

        private bool _fixedDelta;

        private readonly float time;

        public float value
        {
            get
            {
                return _min + Mathf.PingPong(currentTime, minMaxDifference);
            }
        }

        /// <summary>Creates a new pingpong value object with a mininum value and a max value, a pingpong count, speed, whether it should
        /// increase by fixed delta or not and a start perc at which the value should start</summary>
        public PingPongValue(float min, float max, int count, float speed, bool fixedDelta, float startPerc)
        {
            if (min > max)
            {
                throw new System.Exception($"min: {min} is greater than max: {max} :: this is not intended!");
            }

            _min = min;
            _max = max;
            _fixedDelta = fixedDelta;
            _speed = speed;

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

        /// <summary>Creates a new pingpong value object with a mininum value and a max value, a pingpong count, and a speed</summary>
        public PingPongValue(float min, float max, int count, float speed) : this(min, max, count, speed, false, 0.0f)
        {
        }

        /// <summary>Creates a new pingpong value object with a mininum value and a max value, a pingpong count, speed and whether it should
        /// increase by fixed delta</summary>
        public PingPongValue(float min, float max, int count, float speed, bool fixedDelta) : this(min, max, count, speed, fixedDelta, 0.0f)
        {
        }

        /// <summary>Creates a new pingpong value object with a mininum value and a max value, a pingpong count, speed, and a start perc at which the value should start</summary>
        public PingPongValue(float min, float max, int count, float speed, float startPerc) : this(min, max, count, speed, false, startPerc)
        {
        }

        /// <summary>
        /// Returns if set count hasn't been reached. Increments current time by delta if this is the case
        /// </summary>
        public bool Continue()
        {
            if (currentTime == time)
            {
                return false;
            }

            currentTime += (_fixedDelta ? Time.fixedDeltaTime : Time.deltaTime) * _speed;
            if (currentTime > time)
            {
                currentTime = time;
            }
            return true;
        }
    }
}