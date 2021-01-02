// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// Manages the loading interface when a loading screen is requested
    /// </summary>
    public class LoadScreenManager : MonoBehaviour
    {
        [Header("Loading Screen")]
        [SerializeField]
        private GameObject loadingInterface = null;

        [SerializeField]
        private Image loadingProgressBar = null;

        [Header("Channel listening to")]
        [SerializeField]
        private ASyncOperationsEventChannelSO loadScreenEventChannel = null;

        private void OnEnable()
        {
            loadingProgressBar.fillAmount = 0.0f;

            loadScreenEventChannel.OnRequestRaised += StartLoadScreen;
        }

        private void OnDisable()
        {
            loadScreenEventChannel.OnRequestRaised -= StartLoadScreen;
        }

        /// <summary>
        /// Starts the load screen process using the given load operations
        /// </summary>
        /// <param name="loadOperations"></param>
        private void StartLoadScreen(AsyncOperation[] loadOperations)
        {
            loadingInterface.SetActive(true);

            StartCoroutine(UpdateLoadScreen(loadOperations));
        }

        /// <summary>
        /// Returns an enumerator that updates the loading progress on screen using given load operations
        /// </summary>
        private IEnumerator UpdateLoadScreen(AsyncOperation[] loadOperations)
        {
            float totalProgress = 0.0f;

            // When the scene reaches 0.9f, it means that it is loaded
            // The remaining 0.1f are for the integration
            while (totalProgress <= 0.9f)
            {
                totalProgress = 0.0f;

                foreach (AsyncOperation operation in loadOperations)
                {
                    totalProgress += operation.progress;
                }

                totalProgress = Mathf.Clamp01(totalProgress / loadOperations.Length);
                loadingProgressBar.fillAmount = totalProgress /= loadOperations.Length;

                yield return null;
            }

            loadingInterface.SetActive(false);
        }
    }
}