using System;
using UnityEngine;

namespace BWolf.Utilities
{
    [Serializable]
    public class Vector2Lerp : LerpOf<Vector2>
    {
        public Vector2Lerp() => lerpFunction = Vector2.Lerp;
    }
    
    [Serializable]
    public class Vector3Lerp : LerpOf<Vector3>
    {
        public Vector3Lerp() => lerpFunction = Vector3.Lerp;
    }
    
    [Serializable]
    public class QuaternionLerp : LerpOf<Quaternion>
    {
        public QuaternionLerp() => lerpFunction = Quaternion.Lerp;
    }

    [Serializable]
    public class FloatLerp : LerpOf<float>
    {
        public FloatLerp() => lerpFunction = Mathf.Lerp;
    }
}
