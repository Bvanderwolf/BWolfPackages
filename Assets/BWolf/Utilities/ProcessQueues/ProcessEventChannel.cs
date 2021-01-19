// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using System.Collections.Generic;
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
        public Action<TProcessInfo> OnCallbackRaised;

        public void RaiseEvent(TProcessInfo info)
        {
            OnCallbackRaised?.Invoke(info);
        }
    }
}