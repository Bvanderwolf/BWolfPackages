// Created By: Benjamin van der Wolf
// Version: 1.1
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.LerpValue
{
    /// <summary>Representation of lerp values</summary>
    public struct LerpValue<T> where T : struct
    {
        public readonly T Start;
        public readonly T End;
        public readonly float Time;

        private float currentTime;
        private LerpSetting setting;

        public float Perc
        {
            get
            {
                return setting(currentTime, Time);
            }
        }

        public LerpValue(T start, T end, float time, float currentTime = 0)
        {
            Start = start;
            End = end;
            Time = time;
            setting = LerpSettings.Default;

            this.currentTime = currentTime;
        }

        public LerpValue(T start, T end, float time, LerpSetting setting, float currentTime = 0)
        {
            Start = start;
            End = end;
            Time = time;

            this.setting = setting;
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

    public static class LerpSettings
    {
        public readonly static LerpSetting Default = (c, t) => c / t;
        public readonly static LerpSetting Sine = (c, t) => Mathf.Sin(c / t * Mathf.PI * 0.5f);
        public readonly static LerpSetting Cosine = (c, t) => 1f - Mathf.Cos(c / t * Mathf.PI * 0.5f);
    }

    public delegate float LerpSetting(float current, float time);
}