﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Behaviours
{
    /// <summary>
    /// A singleton class to be derived from by monobehaviours to make use of the singleton functionality but keep monobehaviour functionalities like coroutines
    /// </summary>
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
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
        protected SingletonBehaviour()
        {
        }

        protected virtual void OnDestroy()
        {
            appIsQuitting = true;
        }

        /// <summary>Returns singleton instance by constructing a undestroyable gameobject with this component attached to it</summary>
        private static T CreateSingleton()
        {
            var gameObject = new GameObject($"{typeof(T).Name} (singleton)");
            var instance = gameObject.AddComponent<T>();
            DontDestroyOnLoad(gameObject);
            return instance;
        }
    }
}