using BWolf.Behaviours;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.SceneTransitioning
{
    public class SceneTransitionSystem : LazySingletonBehaviour<SceneTransitionSystem>
    {
        public void Transition(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void Transition(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}