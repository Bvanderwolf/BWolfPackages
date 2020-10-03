// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>A behaviour for creating a scene transition effect using various UI elements and enumerators to be used as coroutines</summary>
    public class FadeTransitionProvider : MonoBehaviour, ITransitionProvider
    {
        [Header("Settings")]
        [SerializeField]
        private string nameOfTransition = "Fade";

        [SerializeField]
        private float transitionTime = 2.5f;

        [Header("Settings")]
        [SerializeField]
        private CanvasGroup group = null;

        [SerializeField]
        private Image imgLoadBar = null;

        public string TransitionName
        {
            get { return nameOfTransition; }
        }

        /// <summary>Returns an enumerator that linearly interpolates the alpha of the canvasgroup from 0 to 1</summary>
        public IEnumerator Outro()
        {
            LerpValue<float> alphaLerp = new LerpValue<float>(0, 1, transitionTime);

            while (alphaLerp.Continue())
            {
                group.alpha = Mathf.Lerp(alphaLerp.start, alphaLerp.end, alphaLerp.perc);
                yield return null;
            }
        }

        /// <summary>Returns an enumerator that linearly interpolates the alpha of the canvasgroup from 1 to 0</summary>
        public IEnumerator Intro()
        {
            LerpValue<float> alphaLerp = new LerpValue<float>(1, 0, transitionTime);

            while (alphaLerp.Continue())
            {
                group.alpha = Mathf.Lerp(alphaLerp.start, alphaLerp.end, alphaLerp.perc);
                yield return null;
            }
        }

        /// <summary>Sets the given percentage value as fillamount on stored loadbar image</summary>
        public void OnProgressUpdated(float perc)
        {
            imgLoadBar.fillAmount = perc;
        }
    }
}