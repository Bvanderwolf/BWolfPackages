// Created By: Benjamin van der Wolf
// Version: 1.0
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
        /// Returns if current time has reached time value. Increments current time by t if this is not the case
        /// </summary>
        public bool IncrementTime(float t)
        {
            if (currentTime == Time)
            {
                return true;
            }

            currentTime += t;
            if (currentTime > Time)
            {
                currentTime = Time;
            }

            return false;
        }
    }
}