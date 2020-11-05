using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>Utility script for providing a scene transition starting functionality to a button</summary>
    [RequireComponent(typeof(Button))]
    public class SceneTransitionButton : MonoBehaviour
    {
        [SerializeField]
        private string sceneName = string.Empty;

        [SerializeField]
        private string transitionName = string.Empty;

        [SerializeField]
        private LoadSceneMode mode = LoadSceneMode.Additive;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(StartSceneTransition);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(StartSceneTransition);
        }

        private void StartSceneTransition()
        {
            var system = SceneTransitionSystem.Instance;
            if (system.SceneIsLoadable(sceneName))
            {
                system.Transition(transitionName, sceneName, mode);
            }
        }
    }
}