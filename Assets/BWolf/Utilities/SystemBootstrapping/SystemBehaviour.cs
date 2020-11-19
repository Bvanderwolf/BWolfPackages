// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    /// <summary>The class from which systems can derive to be used by the system locator</summary>
    [DisallowMultipleComponent]
    public abstract class SystemBehaviour : MonoBehaviour
    {
        protected virtual void Reset()
        {
            string type = GetType().Name;
            if (gameObject.name != type)
            {
                Debug.LogWarning($"Name Of {type} and its gameObject name should be the same");
                gameObject.name = type;
            }
        }
    }
}