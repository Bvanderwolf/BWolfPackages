// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
// Dependencies: LerpValue
//----------------------------------

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
        private FadeTransitionProvider transitionProvider = null;

        [Header("References")]
        [SerializeField]
        private Button btnSwitch = null;

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
            SceneTransitionSystem.Instance.Transition(transitionProvider, namesOfScenes[indexOfSceneAt++], LoadSceneMode.Additive);

            if (indexOfSceneAt == namesOfScenes.Length)
            {
                indexOfSceneAt = 0;
            }

            SetButtonText();
        }

        private void SetButtonText()
        {
            txtTransition.text = "Transition to: " + namesOfScenes[indexOfSceneAt];
        }
    }
}