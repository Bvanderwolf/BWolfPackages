using BWolf.Utilities;
using BWolf.Utilities.SceneTransitioning;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BWolf.Examples.SceneTransitioning
{
    public class SceneSwitchUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string[] namesOfScenes = null;

        [SerializeField]
        private float transitionTime = 2.5f;

        [Header("References")]
        [SerializeField]
        private Button btnSwitch = null;

        [SerializeField]
        private CanvasGroup group = null;

        [SerializeField]
        private Image imgLoadBar = null;

        [SerializeField]
        private Text txtTransition = null;

        private int indexOfSceneAt = 1;

        private void Awake()
        {
            SetButtonText();

            btnSwitch.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SceneTransitionSystem.Instance.Transition(namesOfScenes[indexOfSceneAt++], LoadSceneMode.Additive)
                .AddOutroRoutine(Outro())
                .AddIntroRoutine(Intro())
                .OnProgressUpdated(OnProgresUpdate);

            if (indexOfSceneAt == namesOfScenes.Length)
            {
                indexOfSceneAt = 0;
            }

            imgLoadBar.fillAmount = 0.0f;
            SetButtonText();
        }

        private IEnumerator Outro()
        {
            LerpValue<float> alphaLerp = new LerpValue<float>(0, 1, transitionTime);

            while (alphaLerp.Continue())
            {
                group.alpha = Mathf.Lerp(alphaLerp.start, alphaLerp.end, alphaLerp.perc);
                yield return null;
            }
        }

        private IEnumerator Intro()
        {
            LerpValue<float> alphaLerp = new LerpValue<float>(1, 0, transitionTime);

            while (alphaLerp.Continue())
            {
                group.alpha = Mathf.Lerp(alphaLerp.start, alphaLerp.end, alphaLerp.perc);
                yield return null;
            }
        }

        private void OnProgresUpdate(float perc)
        {
            imgLoadBar.fillAmount = perc;
        }

        private void SetButtonText()
        {
            txtTransition.text = "Transition to: " + namesOfScenes[indexOfSceneAt];
        }
    }
}