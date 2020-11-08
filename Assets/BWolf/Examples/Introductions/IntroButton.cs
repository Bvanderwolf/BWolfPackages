using BWolf.Utilities.Introductions;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.Introductions
{
    [RequireComponent(typeof(Button))]
    public class IntroButton : MonoBehaviour
    {
        [SerializeField]
        private Introduction introductionOnClick = null;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(StartIntroduction);
        }

        private void OnDestroy()
        {
            button.onClick.AddListener(StartIntroduction);
        }

        private void StartIntroduction()
        {
            if (!IntroductionManager.Instance.IsActive && !introductionOnClick.Finished)
            {
                introductionOnClick.Start();
            }
        }
    }
}