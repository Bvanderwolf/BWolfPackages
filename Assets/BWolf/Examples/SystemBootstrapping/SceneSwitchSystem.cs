using BWolf.Utilities.SystemBootstrapping;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.SystemBootstrapping
{
    public class SceneSwitchSystem : SystemBehaviour
    {
        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        private AsyncOperation switchOperation;

        public float SwitchPercentage
        {
            get { return switchOperation != null ? 0.0f : switchOperation.progress; }
        }

        public void Switch(string nameOfNewScene)
        {
            if (string.IsNullOrEmpty(nameOfNewScene))
            {
                throw new System.InvalidOperationException($"scene with name: {nameOfNewScene} is not a valid name");
            }

            StartCoroutine(SwitchToScene(SceneManager.LoadSceneAsync(nameOfNewScene, loadSceneMode)));
        }

        private IEnumerator SwitchToScene(AsyncOperation switchOperation)
        {
            this.switchOperation = switchOperation;

            while (!switchOperation.isDone)
            {
                yield return null;
            }

            this.switchOperation = null;
        }
    }
}