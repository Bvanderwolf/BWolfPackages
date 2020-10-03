// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities;
using BWolf.Utilities.SceneTransitioning;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.SceneTransitioning
{
    /// <summary>A behaviour for creating a scene transition effect using various UI elements and enumerators to be used as coroutines</summary>
    public class SlideTransitionProvider : MonoBehaviour, ITransitionProvider
    {
        [Header("Settings")]
        [SerializeField]
        private string nameOfTransition = "Slide";

        [SerializeField]
        private float transitionTime = 1.25f;

        [Header("References")]
        [SerializeField]
        private Image imgBackground = null;

        [SerializeField]
        private GameObject loadbar = null;

        [SerializeField]
        private Image imgLoadBar = null;

        public string TransitionName
        {
            get { return nameOfTransition; }
        }

        /// <summary>Returns an enumerator that linearly interpolates the alpha of the canvasgroup from 0 to 1</summary>
        public IEnumerator Outro()
        {
            LerpValue<float> leftSlide = new LerpValue<float>(0, 1, transitionTime, LerpSettings.Cosine);

            while (leftSlide.Continue())
            {
                imgBackground.fillAmount = Mathf.Lerp(leftSlide.start, leftSlide.end, leftSlide.perc);
                yield return null;
            }

            loadbar.SetActive(true);
        }

        /// <summary>Returns an enumerator that linearly interpolates the alpha of the canvasgroup from 1 to 0</summary>
        public IEnumerator Intro()
        {
            loadbar.SetActive(false);

            LerpValue<float> rightSlide = new LerpValue<float>(1, 0, transitionTime, LerpSettings.Cosine);

            while (rightSlide.Continue())
            {
                imgBackground.fillAmount = Mathf.Lerp(rightSlide.start, rightSlide.end, rightSlide.perc);
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