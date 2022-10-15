using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.SceneSearch
{
    /// <summary>
    /// UPDATE:
    ///  - Currently: root game objects are cached and traversal is done upon invocation of Find method.
    ///  - New method: traversal of stored paths is done when a new scene is loaded. Transforms at the end
    ///    of each path are cached. Upon invocation of the Find method, we can loop through this cache. 
    /// </summary>
    public readonly partial struct ScenePath
    {
        private static ScenePath[] _paths;
        
        private static readonly List<int> _loadedScenePathBuildIndices;

        private static readonly List<GameObject> _rootGameObjects;

        private static readonly IEnumerable<GameObject> _filterRootGameObjects;

        static ScenePath()
        {
            _paths = Array.Empty<ScenePath>();
            _loadedScenePathBuildIndices = new List<int>();
            _rootGameObjects = new List<GameObject>();
            _filterRootGameObjects = _rootGameObjects
                .Where(root => _loadedScenePathBuildIndices.Contains(root.scene.buildIndex))
                .Where(r => _paths.Any(p => r.name == p._root));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadScenePathConfiguration()
        {
            ScenePathConfig config = Resources.Load<ScenePathConfig>(nameof(ScenePathConfig));
            if (config == null)
                return;

            _paths = config.GetGameObjectPaths().Select(p => new ScenePath(p)).ToArray();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            // Remove the unloaded root game objects.
            for (int i = _rootGameObjects.Count - 1; i >= 0; i--)
                if (_rootGameObjects[i] == null)
                    _rootGameObjects.RemoveAt(i);

            // Remove cached scene data if its part of stored paths.
            if (_paths.Any(p => p._scene == scene.name))
                _loadedScenePathBuildIndices.Remove(scene.buildIndex);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Populate root game objects cache with root game objects of scene.
            scene.GetRootGameObjects(_rootGameObjects);
            
            // Cache scene data if its part of stored paths.
            if (_paths.Any(p => p._scene == scene.name))
                _loadedScenePathBuildIndices.Add(scene.buildIndex);
        }

        public static GameObject Find(string name)
        {
            IEnumerable<GameObject> roots = _filterRootGameObjects;

            foreach (GameObject root in roots)
            {
                Transform child = FindRecursive(root.transform, name);
                if (child != null)
                    return child.gameObject;
            }

            return null;
        }

        public static Transform FindRecursive(Transform transform, string name)
        {
            if (transform.name == name)
                return transform;
            
            int childCount = transform.childCount;
            if (childCount == 0)
                return null;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = FindRecursive(transform.GetChild(i), name);
                if (child != null)
                    return child;
            }

            return null;
        }
    }
}
