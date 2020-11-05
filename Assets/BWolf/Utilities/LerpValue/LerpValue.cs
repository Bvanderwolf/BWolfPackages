// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.6
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>Representation of lerp values</summary>
    public struct LerpValue<T>
    {
        public readonly T start;
        public readonly T end;

        private float currentTime;
        private LerpSetting setting;
        private float time;
        private bool fixedDelta;

        /// <summary>current percentage at which the value is linearly interpolating</summary>
        public float perc
        {
            get
            {
                return setting(currentTime, time);
            }
        }

        /// <summary>remaining time left to used for linearly interpolating</summary>
        public float remainer
        {
            get
            {
                return time - currentTime;
            }
        }

        public LerpValue(T start, T end, float time) : this(start, end, time, LerpSettings.Default, 0.0f, false)
        {
        }

        public LerpValue(T start, T end, float time, LerpSetting setting) : this(start, end, time, setting, 0.0f, false)
        {
        }

        public LerpValue(T start, T end, float time, LerpSetting setting, float startPerc, bool fixedDelta)
        {
            this.start = start;
            this.end = end;
            this.time = time;
            this.setting = setting;
            this.fixedDelta = fixedDelta;

            currentTime = time * Mathf.Clamp01(startPerc);
        }

        /// <summary>
        /// Returns if current time hasn't reached stored "time" value. Increments current time if this is the case
        /// </summary>
        public bool Continue()
        {
            if (currentTime == time)
            {
                return false;
            }

            currentTime += (!fixedDelta ? Time.deltaTime : Time.fixedDeltaTime);
            if (currentTime > time)
            {
                currentTime = time;
            }

            return true;
        }
    }

    /// <summary>Exposes lerp settings to be used when using lerp value</summary>
    public static class LerpSettings
    {
        /// <summary>No curve is applied to the linear interpolation</summary>
        public readonly static LerpSetting Default = (c, t) => c / t;

        /// <summary>Curve that makes linear interpolation start fast and end slow</summary>
        public readonly static LerpSetting Sine = (c, t) => Mathf.Sin(c / t * Mathf.PI * 0.5f);

        /// <summary>Curve that makes linear interpolation start slow and end fast</summary>
        public readonly static LerpSetting Cosine = (c, t) => 1f - Mathf.Cos(c / t * Mathf.PI * 0.5f);
    }

    /// <summary>Setting used for returning a "modified" percentage for a lerp value</summary>
    public delegate float LerpSetting(float current, float time);
}