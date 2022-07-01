using System;
using System.Collections;
using BWolf.Utilities;
using UnityEngine;

public static class LerpExtensions
{
    public static IEnumerator Await<T>(this LerpOf<T> value, Action<T> callback) => Await(value, null, callback);

    public static IEnumerator Await<T>(this LerpOf<T> lerp, YieldInstruction instruction, Action<T> callback)
    {
        do
        {
            callback.Invoke(lerp.Value);
            yield return instruction;
                
        } while (lerp.Continue());
    }
}
