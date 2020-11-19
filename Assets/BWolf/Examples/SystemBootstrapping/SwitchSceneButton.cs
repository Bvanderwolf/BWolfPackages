using BWolf.Utilities.SystemBootstrapping;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.SystemBootstrapping
{
    public class SwitchSceneButton : MonoBehaviour
    {
        [SerializeField]
        private string nameOfScene = string.Empty;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(SwitchScene);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(SwitchScene);
        }

        private void SwitchScene()
        {
            SystemLocator.Instance.Get<SceneSwitchSystem>().Switch(nameOfScene);
        }
    }
}