using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Examples.SceneTransitioning
{
    public class LoadUIOnStartBehaviour : MonoBehaviour
    {
        private const string nameOfUIScene = "DemoUIScene";

        private void Start()
        {
            if (!SceneManager.GetSceneByName(nameOfUIScene).isLoaded)
            {
                StartCoroutine(LoadSceneASync());
            }
        }

        private IEnumerator LoadSceneASync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nameOfUIScene, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}