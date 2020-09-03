// Created By: Benjamin van der Wolf
// Version: 1.5
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
        private float speed;
        private bool fixedDelta;

        public float perc
        {
            get
            {
                return setting(currentTime, time);
            }
        }

        public LerpValue(T start, T end, float time) : this(start, end, time, 1f, LerpSettings.Default, 0f, false)
        {
        }

        public LerpValue(T start, T end, float time, float speed) : this(start, end, time, speed, LerpSettings.Default, 0f, false)
        {
        }

        public LerpValue(T start, T end, float time, LerpSetting setting) : this(start, end, time, 1f, setting, 0f, false)
        {
        }

        public LerpValue(T start, T end, float time, float speed, LerpSetting setting) : this(start, end, time, speed, setting, 0f, false)
        {
        }

        public LerpValue(T start, T end, float time, float speed, LerpSetting setting, float startPerc = 0f, bool fixedDelta = false)
        {
            this.start = start;
            this.end = end;
            this.time = time;
            this.speed = speed;
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

            currentTime += (!fixedDelta ? Time.deltaTime : Time.fixedDeltaTime) * speed;
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