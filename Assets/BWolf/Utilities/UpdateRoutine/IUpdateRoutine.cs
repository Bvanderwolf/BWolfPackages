using System;
using UnityEngine;

public interface IUpdateRoutine<out T>
{
    IUpdateRoutine<T> SetDispatcher(MonoBehaviour monoBehaviour);

    IUpdateRoutine<T> OnUpdate(Action<T> action);

    void Start();

    void Stop();
}
