using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BWolf.MeshSelecting
{
    /// <summary>
    /// Provides a global api for mesh selection done with the GUI.DrawTexture method.
    /// It works with the <see cref="MeshSelector"/> behaviour.
    /// </summary>
    public static class MeshSelection
    {
        /// <summary>
        /// The camera used to cast from.
        /// </summary>
        public static Camera SelectionCamera
        {
            get => _camera ?? Camera.main;
            set
            {
                if (_behaviour == null)
                    SetActive(false);
                
                _camera = value;
                _behaviour.SelectionCamera = _camera;
            }
        }

        /// <summary>
        /// Whether a drag selection is currently in progress.
        /// </summary>
        public static bool IsDragSelecting => _behaviour != null && _behaviour.IsDragSelecting;

        /// <summary>
        /// The selection condition determining if a found collider is fit for selection.
        /// Will select everything if not set.
        /// </summary>
        public static Func<Collider, bool> SelectionCondition
        {
            set
            {
                if (_behaviour == null)
                    SetActive(false);

                _behaviour.SelectionCondition = value;
            }
        }

        /// <summary>
        /// Fired when an object is clicked.
        /// </summary>
        public static event Action<GameObject> Clicked;

        /// <summary>
        /// Fired when the selection has changed after
        /// a box selection has ended.
        /// </summary>
        public static event Action SelectionChanged;

        /// <summary>
        /// Whether the mesh selection is active.
        /// </summary>
        public static bool IsActive => _behaviour != null && _behaviour.enabled;

        /// <summary>
        /// The currently selected game objects.
        /// </summary>
        public static GameObject[] Selection => !IsActive ? Array.Empty<GameObject>() : _behaviour.Selection;

        /// <summary>
        /// The camera used for box selection. If this value is null, Camera.main will be used.
        /// </summary>
        private static Camera _camera;
        
        /// <summary>
        /// The settings used for creating a new mesh selector if none is found
        /// in any scene during activation.
        /// </summary>
        private static SelectionSettings _settings;

        /// <summary>
        /// The mesh selector instance.
        /// </summary>
        private static MeshSelector _behaviour;
        
        /// <summary>
        /// Called when the app is loading to set the settings reference.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnAppLoad()
        {
            _settings = Resources.Load<SelectionSettings>(nameof(SelectionSettings)) ??
                        ScriptableObject.CreateInstance<SelectionSettings>();
        }

        /// <summary>
        /// Sets whether mesh selection is active or not.
        /// </summary>
        /// <param name="value">The active value of mesh selection.</param>
        public static void SetActive(bool value)
        {
            if (_behaviour == null)
                _behaviour = FindOrCreateBehaviourLazy();

            _behaviour.enabled = value;
        }

        /// <summary>
        /// Searches for a mesh selector instance and if not found creates one using found settings.
        /// </summary>
        /// <returns>The found or creates mesh selector instance.</returns>
        private static MeshSelector FindOrCreateBehaviourLazy()
        {
            _behaviour = Object.FindObjectOfType<MeshSelector>() ?? MeshSelector.CreateFromSettings(_settings);
            _behaviour.Clicked += Clicked;
            _behaviour.SelectionChanged += SelectionChanged;

            return _behaviour;
        }
    }
}
