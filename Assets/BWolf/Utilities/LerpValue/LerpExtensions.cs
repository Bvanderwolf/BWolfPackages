using System;
using System.Collections;
using BWolf.Utilities;
using UnityEngine;

public static class LerpExtensions
{
    public static IEnumerator Yield<T>(this Lerp<T> value, Action<T> callback) => Yield(value, null, callback);

    public static IEnumerator Yield<T>(this Lerp<T> value, YieldInstruction instruction, Action<T> callback)
    {
        do
        {
            callback.Invoke(value.Value);
            yield return instruction;
                
        } while (value.Continue());
    }
}
