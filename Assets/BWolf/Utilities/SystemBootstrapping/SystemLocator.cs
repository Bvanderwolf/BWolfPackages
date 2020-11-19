using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    public class SystemLocator
    {
        public static SystemLocator Instance { get; private set; }

        private SystemLocator()
        {
        }

        private Dictionary<string, SystemBehaviour> systems = new Dictionary<string, SystemBehaviour>();

        public static void Awake()
        {
            if (Instance != null)
            {
                throw new System.InvalidOperationException("Cannot re-awake a system locator");
            }

            Instance = new SystemLocator();
        }

        public T Get<T>() where T : SystemBehaviour
        {
            string key = typeof(T).Name;
            if (!systems.ContainsKey(key))
            {
                throw new System.InvalidOperationException($"{key} is not registered with the system locator");
            }

            return (T)systems[key];
        }

        public void Register<T>() where T : SystemBehaviour
        {
            string key = typeof(T).Name;
            if (systems.ContainsKey(key))
            {
                throw new System.InvalidOperationException($"{key} is already registered with the system locator");
            }

            var system = Resources.Load<T>("Systems/" + typeof(T).Name);
            systems.Add(key, system);

            var gameObject = GameObject.Instantiate(system.gameObject);
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}