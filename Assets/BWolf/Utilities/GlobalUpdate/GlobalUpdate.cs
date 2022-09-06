using System;
using UnityEngine;

namespace BWolf.GlobalUpdateEvent
{
    public class GlobalUpdate : MonoBehaviour
    {
        private static GlobalUpdate _instance;
        
        private static Action _updates;
        private void Update()
        {
            _updates?.Invoke();
        }

        public static void On(Action action)
        {
            EnsureInstance();
            
            _updates += action;
        }

        private static void EnsureInstance()
        {
            if (_instance != null)
                return;

            GameObject gameObject = new GameObject("~GlobalUpdate");
            _instance = gameObject.AddComponent<GlobalUpdate>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
