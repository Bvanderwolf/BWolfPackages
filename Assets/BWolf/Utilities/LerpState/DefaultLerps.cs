﻿using System;
using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents continuable linear interpolation operation data of a Vector2.
    /// Uses Unity's Vector2.Lerp function by default.
    /// </summary>
    [Serializable]
    public class Vector2Lerp : LerpOf<Vector2>
    {
        public Vector2Lerp( 
            Vector2 initial, 
            Vector2 target, 
            float totalTime = 1.0f, 
            EasingFunction easingFunction = null,
            TimeOverride timeOverride = TimeOverride.KEEP_CURRENT_TIME,
            bool usesFixedDelta = false) 
            : base(initial, target, totalTime, easingFunction, Vector2.Lerp, timeOverride, usesFixedDelta)
        {
        }
        
        public Vector2Lerp() => lerpFunction = Vector2.Lerp;
    }
    
    /// <summary>
    /// Represents continuable linear interpolation operation data of a Vector3.
    /// Uses Unity's Vector3.Lerp function by default.
    /// </summary>
    [Serializable]
    public class Vector3Lerp : LerpOf<Vector3>
    {
        public Vector3Lerp( 
            Vector3 initial, 
            Vector3 target, 
            float totalTime = 1.0f, 
            EasingFunction easingFunction = null,
            TimeOverride timeOverride = TimeOverride.KEEP_CURRENT_TIME,
            bool usesFixedDelta = false) 
            : base(initial, target, totalTime, easingFunction, Vector3.Lerp, timeOverride, usesFixedDelta)
        {
        }
        
        public Vector3Lerp() => lerpFunction = Vector3.Lerp;
    }
    
    /// <summary>
    /// Represents continuable linear interpolation operation data of a Quaternion.
    /// Uses Unity's Quaternion.Lerp function by default.
    /// </summary>
    [Serializable]
    public class QuaternionLerp : LerpOf<Quaternion>
    {
        public QuaternionLerp( 
            Quaternion initial, 
            Quaternion target, 
            float totalTime = 1.0f, 
            EasingFunction easingFunction = null,
            TimeOverride timeOverride = TimeOverride.KEEP_CURRENT_TIME,
            bool usesFixedDelta = false) 
            : base(initial, target, totalTime, easingFunction, Quaternion.Lerp, timeOverride, usesFixedDelta)
        {
        }
        
        public QuaternionLerp() => lerpFunction = Quaternion.Lerp;
    }

    /// <summary>
    /// Represents continuable linear interpolation operation data of a floating point value.
    /// Uses Unity's Mathf.Lerp function by default.
    /// </summary>
    [Serializable]
    public class FloatLerp : LerpOf<float>
    {
        public FloatLerp( 
            float initial, 
            float target, 
            float totalTime = 1.0f, 
            EasingFunction easingFunction = null,
            TimeOverride timeOverride = TimeOverride.KEEP_CURRENT_TIME,
            bool usesFixedDelta = false)
            : base(initial, target, totalTime, easingFunction, Mathf.Lerp, timeOverride, usesFixedDelta)
        {
        }
        
        public FloatLerp() => lerpFunction = Mathf.Lerp;
    }
}