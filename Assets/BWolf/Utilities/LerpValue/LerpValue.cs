// Created By: Benjamin van der Wolf
// Version: 1.1
//----------------------------------

namespace BWolf.Utilities.LerpValue
{
    /// <summary>Representation of lerp values</summary>
    public struct LerpValue<T> where T : struct
    {
        public readonly T Start;
        public readonly T End;
        public readonly float Time;

        private float currentTime;

        public float Perc
        {
            get
            {
                return currentTime / Time;
            }
        }

        public LerpValue(T start, T end, float time, float currentTime = 0)
        {
            Start = start;
            End = end;
            Time = time;

            this.currentTime = currentTime;
        }

        /// <summary>
        /// Returns if current time hasn't reached stored "Time" value. Increments current time by t if this is the case
        /// </summary>
        public bool Continue(float t)
        {
            if (currentTime == Time)
            {
                return false;
            }

            currentTime += t;
            if (currentTime > Time)
            {
                currentTime = Time;
            }

            return true;
        }
    }
}