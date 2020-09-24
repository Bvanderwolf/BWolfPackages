using BWolf.Behaviours.SingletonBehaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.SceneTransitioning
{
    public class SceneTransitionSystem : SingletonBehaviour<SceneTransitionSystem>
    {
        private string[] scenesInBuild;
        private bool transitioning;

        protected override void Awake()
        {
            base.Awake();

            int sceneCount = SceneManager.sceneCountInBuildSettings;
            scenesInBuild = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild[i] = (scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }
        }

        public void Transition(int sceneIndex, LoadSceneMode mode)
        {
            if (!transitioning)
            {
                if (sceneIndex >= 0 && sceneIndex < scenesInBuild.Length)
                {
                    StartCoroutine(TransitionRoutine(scenesInBuild[sceneIndex], mode));
                }
            }
        }

        public void Transition(string sceneName, LoadSceneMode mode)
        {
            if (!transitioning)
            {
                if (SceneIsLoadable(sceneName))
                {
                    StartCoroutine(TransitionRoutine(sceneName, mode));
                }
            }
        }

        public void Transition(string sceneName, LoadSceneMode mode, IEnumerator enumerator)
        {
            if (!transitioning)
            {
                if (SceneIsLoadable(sceneName))
                {
                    StartCoroutine(TransitionRoutine(sceneName, mode, enumerator));
                }
            }
        }

        private bool SceneIsLoadable(string sceneName)
        {
            for (int i = 0; i < scenesInBuild.Length; i++)
            {
                if (sceneName == scenesInBuild[i])
                {
                    return true;
                }
            }

            Debug.LogWarning("Scene with sceneName: " + sceneName + " is not loadable because it doesn't exist");
            return false;
        }

        private IEnumerator TransitionRoutine(string sceneName, LoadSceneMode mode)
        {
            transitioning = true;

            print(SceneManager.GetActiveScene().name);

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            transitioning = false;
        }

        private IEnumerator TransitionRoutine(string sceneName, LoadSceneMode mode, IEnumerator enumerator)
        {
            transitioning = true;

            yield return enumerator;

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            transitioning = false;
        }
    }
}