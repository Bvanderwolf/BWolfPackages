using BWolf.Utilities.SceneTransitioning;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.SceneTransitioning
{
    public class SceneSwitchButton : MonoBehaviour
    {
        [SerializeField]
        private string nameOfScene = string.Empty;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SceneTransitionSystem.Instance.Transition(nameOfScene);
            });
        }
    }
}