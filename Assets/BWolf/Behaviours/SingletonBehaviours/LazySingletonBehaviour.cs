// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Behaviours.SingletonBehaviours
{
    /// <summary>
    /// A lazy singleton class to be derived from by monobehaviours to make use of the singleton functionality but keep monobehaviour functionalities like coroutines, Make sure to only
    /// use this when your singleton has no inspector fields and thus is not already in a scene.
    /// </summary>
    public class LazySingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private readonly static Lazy<T> lazyInstance = new Lazy<T>(CreateSingleton);

        private static bool appIsQuitting;

        public static T Instance
        {
            get
            {
                if (appIsQuitting)
                {
                    //if this object is already destroyed during app closage return null to avoid MissingReferenceException
                    return null;
                }

                return lazyInstance.Value;
            }
        }

        //make sure no instance of this object can be made using new keyword by making the constructor protected
        protected LazySingletonBehaviour()
        {
        }

        protected virtual void OnDestroy()
        {
            appIsQuitting = true;
        }

        /// <summary>Returns singleton instance by first searching object of type T in scene and otherwise creating one if not was found</summary>
        private static T CreateSingleton()
        {
            var gameObject = new GameObject($"{typeof(T).Name} (LazySingleton)");
            var instance = gameObject.AddComponent<T>();
            DontDestroyOnLoad(gameObject);
            return instance;
        }
    }
}