// Created By: Benjamin van der Wolf
// Version: 1.3
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.LerpValue
{
    /// <summary>Representation of lerp values</summary>
    public struct LerpValue<T> where T : struct
    {
        public readonly T start;
        public readonly T end;

        private float currentTime;
        private LerpSetting setting;
        private float time;

        public float perc
        {
            get
            {
                return setting(currentTime, time);
            }
        }

        public LerpValue(T start, T end, float time, float currentTime = 0)
        {
            this.start = start;
            this.end = end;
            this.time = time;
            this.currentTime = currentTime;

            setting = LerpSettings.Default;
        }

        public LerpValue(T start, T end, float time, LerpSetting setting, float currentTime = 0)
        {
            this.start = start;
            this.end = end;
            this.time = time;
            this.setting = setting;
            this.currentTime = currentTime;
        }

        /// <summary>
        /// Returns if current time hasn't reached stored "time" value. Increments current time by delta if this is the case
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

    /// <summary>Exposes lerp settings to be used when using lerp value</summary>
    public static class LerpSettings
    {
        public readonly static LerpSetting Default = (c, t) => c / t;
        public readonly static LerpSetting Sine = (c, t) => Mathf.Sin(c / t * Mathf.PI * 0.5f);
        public readonly static LerpSetting Cosine = (c, t) => 1f - Mathf.Cos(c / t * Mathf.PI * 0.5f);
    }

    /// <summary>Setting used for returning a "modified" percentage for a lerp value</summary>
    public delegate float LerpSetting(float current, float time);
}