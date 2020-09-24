using BWolf.Utilities.SceneTransitioning;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BWolf.Examples.SceneTransitioning
{
    public class SceneSwitchButton : MonoBehaviour
    {
        [SerializeField]
        private string[] namesOfScenes = null;

        [SerializeField]
        private Text txtTransition = null;

        private int indexOfSceneAt = 1;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneTransitionSystem.Instance.Transition(namesOfScenes[indexOfSceneAt++], LoadSceneMode.Additive);

                if (indexOfSceneAt == namesOfScenes.Length)
                {
                    indexOfSceneAt = 0;
                }
            });
        }
    }
}