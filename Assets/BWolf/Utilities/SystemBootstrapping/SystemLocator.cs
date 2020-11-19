// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    public class SystemLocator
    {
        /// <summary>The system locator instance through which system behaviours are retrievable</summary>
        public static SystemLocator Instance { get; private set; }

        private SystemLocator()
        {
        }

        private Dictionary<string, SystemBehaviour> systems = new Dictionary<string, SystemBehaviour>();

        /// <summary>Sets the SystemLocator singleton instance.</summary>
        public static void Awake()
        {
            if (Instance != null)
            {
                throw new System.InvalidOperationException("Cannot re-awake a system locator");
            }

            Instance = new SystemLocator();
        }

        /// <summary>Returns systembehaviour T</summary>
        public T Get<T>() where T : SystemBehaviour
        {
            string key = typeof(T).Name;
            if (!systems.ContainsKey(key))
            {
                throw new System.InvalidOperationException($"{key} is not registered with the system locator");
            }

            return (T)systems[key];
        }

        /// <summary>Registers system behaviour T to the system locator making it usable through the singleton instance</summary>
        public void Register<T>(bool dontDestroyOnLoad = true) where T : SystemBehaviour
        {
            string key = typeof(T).Name;
            if (systems.ContainsKey(key))
            {
                throw new System.InvalidOperationException($"{key} is already registered with the system locator");
            }

            //load and instantiate prefab using key
            var gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Systems/" + key));

            //add key and T component to systems dictionary
            systems.Add(key, gameObject.GetComponent<T>());

            if (dontDestroyOnLoad)
            {
                //make sure the system is not destroyed when loading new scenes
                GameObject.DontDestroyOnLoad(gameObject);
            }
        }
    }
}