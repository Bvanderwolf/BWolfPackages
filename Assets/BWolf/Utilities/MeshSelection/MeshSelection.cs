using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BWolf.MeshSelecting
{
    public static class MeshSelection
    {
        // dispatching the Mesh Selector
        // 1. adding the selector to a scene.
        // 2. on app load

        public static Camera camera;

        private static SelectionSettings _settings;

        private static MeshSelector _behaviour;
        
        [RuntimeInitializeOnLoadMethod]
        private static void OnAppLoad()
        {
            _settings = Resources.Load<SelectionSettings>(nameof(SelectionSettings)) ??
                        ScriptableObject.CreateInstance<SelectionSettings>();
            
            if (_settings.initializeOnLoad)
                Initialize();
        }

        private static void Initialize()
        {
            
        }

        public static void Enable()
        {
            if (_behaviour == null)
            {
                GameObject gameObject = new GameObject("~MeshSelector");
                _behaviour = gameObject.AddComponent<MeshSelector>();
                
                if (!_settings.disableOnLoad)
                    Object.DontDestroyOnLoad(gameObject);
            }
        }

        public static void Disable()
        {
            if (_behaviour == null)
                return;

            _behaviour.enabled = false;
        }
    }
}
