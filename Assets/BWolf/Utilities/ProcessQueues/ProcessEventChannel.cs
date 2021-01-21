// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.ProcessQueues
{
    /// <summary>
    /// Provides functionality to create a the channel through which your own implementation of
    /// the broadcasting process manager can broadcast its calls
    /// </summary>
    /// <typeparam name="TProcessInfo"></typeparam>
    public abstract class ProcessEventChannel<TProcessInfo> : ScriptableObject
    {
        public Action<TProcessInfo> OnEventRaised;

        public void RaiseEvent(TProcessInfo info)
        {
            if (OnEventRaised != null)
            {
                OnEventRaised(info);
            }
            else
            {
                Debug.LogWarning($"A request has been raised for {typeof(TProcessInfo).Name} but the process manager" +
                    " didnt respond. Make sure it is part of a scene");
            }
        }
    }
}