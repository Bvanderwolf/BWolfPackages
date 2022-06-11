using System;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities
{

    public static class PingPongExtensions
    {
        public static IEnumerator Yield(this PingPong value, Action<float> callback) => Yield(value, null, callback);

        public static IEnumerator Yield(this PingPong value, YieldInstruction instruction, Action<float> callback)
        {
            do
            {
                callback.Invoke(value.Value);
                yield return instruction;

            } while (value.Continue());
        }
    }
}