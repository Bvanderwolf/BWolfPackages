using BWolf.Behaviours.SingletonBehaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>System responsible for providing transitions between scenes</summary>
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

        /// <summary>Starts transition from current active scene to scene with given name. Returns a SceneTransition object
        /// to which and outro and intro retoutine can be added and to which can be listed for progress</summary>
        public SceneTransition Transition(string sceneName, LoadSceneMode mode)
        {
            if (transitioning || !SceneIsLoadable(sceneName))
            {
                return null;
            }

            SceneTransition transition = new SceneTransition();

            switch (mode)
            {
                case LoadSceneMode.Single:
                    StartCoroutine(LoadRoutine(sceneName, mode, transition));
                    break;

                case LoadSceneMode.Additive:
                    StartCoroutine(TransitionRoutine(sceneName, mode, transition));
                    break;
            }

            return transition;
        }

        /// <summary>Starts transition from current active scene to scene with given sceneIndex. Returns a SceneTransition object
        /// to which and outro and intro retoutine can be added and to which can be listed for progress</summary>
        public SceneTransition Transition(int sceneIndex, LoadSceneMode mode)
        {
            if (transitioning || !(sceneIndex >= 0 && sceneIndex < scenesInBuild.Length))
            {
                return null;
            }

            SceneTransition transition = new SceneTransition();

            switch (mode)
            {
                case LoadSceneMode.Single:
                    StartCoroutine(LoadRoutine(scenesInBuild[sceneIndex], mode, transition));
                    break;

                case LoadSceneMode.Additive:
                    StartCoroutine(TransitionRoutine(scenesInBuild[sceneIndex], mode, transition));
                    break;
            }

            return transition;
        }

        /// <summary>Returns whether given scene name corresponds to a scene that is stored in the build settings</summary>
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

        /// <summary>Returns a routine for transitiong from the current active scene to a scene with given name in given load mode and
        /// with given SceneTransition object</summary>
        private IEnumerator TransitionRoutine(string sceneName, LoadSceneMode mode, SceneTransition transition)
        {
            transitioning = true;

            yield return null; //wait one frame to wait for SceneTransition to be initialized

            yield return UnLoadRoutine(transition);
            yield return LoadRoutine(sceneName, mode, transition);

            transitioning = false;
        }

        /// <summary>Returns a routine that unloads the current active scene based on given SceneTransition object</summary>
        private IEnumerator UnLoadRoutine(SceneTransition transition)
        {
            if (transition.OutroEnumerator != null)
            {
                yield return transition.OutroEnumerator;
            }

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!unloadOperation.isDone)
            {
                transition.UpdateProgress(unloadOperation.progress / 2.0f);
                yield return null;
            }
        }

        /// <summary>Returns a routine that loads a scene with given name in given load mode and
        /// with given SceneTransition object</summary>
        private IEnumerator LoadRoutine(string sceneName, LoadSceneMode mode, SceneTransition transition)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!loadOperation.isDone)
            {
                transition.UpdateProgress(mode == LoadSceneMode.Single ? loadOperation.progress : ((1.0f + loadOperation.progress) / 2.0f));
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

            if (transition.IntroEnumerator != null)
            {
                yield return transition.IntroEnumerator;
            }
        }
    }
}