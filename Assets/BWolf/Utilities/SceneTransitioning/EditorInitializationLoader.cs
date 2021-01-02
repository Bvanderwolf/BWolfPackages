using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.SceneTransitioning
{
    public class EditorInitializationLoader : MonoBehaviour
    {
        [Header("Initialization")]
        [SerializeField]
        private SceneInfoSO initializationScene = null;

        private void Start()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).path == initializationScene.path)
                {
                    return;
                }
            }

            //load the initialization scene if it is not yet loaded
            SceneManager.LoadSceneAsync(initializationScene.path, LoadSceneMode.Additive);
        }
    }
}