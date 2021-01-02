using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.SceneTransitioning
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Initialization")]
        [SerializeField]
        private SceneInfoSO initializationScene = null;

        [Header("Load on start")]
        [SerializeField]
        private SceneInfoSO[] mainMenuScenes = null;

        [Header("Channel Broadcasting on")]
        [SerializeField]
        private ASyncOperationsEventChannelSO loadScreenEventChannel = null;

        [Header("Channel listening to")]
        [SerializeField]
        private SceneLoadEventChannelSO loadEventChannel = null;

        private List<AsyncOperation> sceneLoadOperations = new List<AsyncOperation>();

        private List<Scene> scenesToUnload = new List<Scene>();

        private SceneInfoSO activeScene;

        private void OnEnable()
        {
            loadEventChannel.OnRequestRaised += StartSceneLoad;
        }

        private void OnDisable()
        {
            loadEventChannel.OnRequestRaised -= StartSceneLoad;
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().path == initializationScene.path)
            {
                LoadScenes(false, mainMenuScenes);
            }
        }

        public void StartSceneLoad(SceneInfoSO[] scenes, bool showLoadingScreen, bool overwrite)
        {
            if (overwrite)
            {
                PrepareLoadedScenesForUnload();
            }

            LoadScenes(showLoadingScreen, scenes);
            UnloadScenesPreparedForUnload();
        }

        public void StartSceneLoad(SceneInfoSO scene, bool showLoadingScreen, bool overwrite)
        {
            if (overwrite)
            {
                PrepareLoadedScenesForUnload();
            }

            LoadScenes(showLoadingScreen, scene);
            UnloadScenesPreparedForUnload();
        }

        private void LoadScenes(bool showLoadingScreen, params SceneInfoSO[] scenesToLoad)
        {
            activeScene = scenesToLoad[0];

            foreach (SceneInfoSO scene in scenesToLoad)
            {
                if (!SceneIsLoaded(scene.path))
                {
                    sceneLoadOperations.Add(SceneManager.LoadSceneAsync(scene.path, LoadSceneMode.Additive));
                }
            }

            sceneLoadOperations[0].completed += OnActiveSceneLoadOperationCompleted;

            if (showLoadingScreen)
            {
                loadScreenEventChannel.RaiseRequest(sceneLoadOperations.ToArray());
            }

            sceneLoadOperations.Clear();
        }

        /// <summary>
        /// Uses the stored active scene to set the active scene when it has been loaded by the scene manager
        /// </summary>
        private void OnActiveSceneLoadOperationCompleted(AsyncOperation operation)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(activeScene.path));
        }

        /// <summary>
        /// Adds all loaded scenes exept the intialization scene to a list of scene to be unloaded
        /// </summary>
        private void PrepareLoadedScenesForUnload()
        {
            int loadedSceneCount = SceneManager.sceneCount;
            for (int i = 0; i < loadedSceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.path != initializationScene.path)
                {
                    scenesToUnload.Add(scene);
                }
            }
        }

        private void UnloadScenesPreparedForUnload()
        {
            for (int i = 0; i < scenesToUnload.Count; i++)
            {
                SceneManager.UnloadSceneAsync(scenesToUnload[i]);
            }

            scenesToUnload.Clear();
        }

        public static bool SceneIsLoaded(string nameOfScene)
        {
            int loadedSceneCount = SceneManager.sceneCount;
            for (int i = 0; i < loadedSceneCount; i++)
            {
                if (nameOfScene == SceneManager.GetSceneAt(i).name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}