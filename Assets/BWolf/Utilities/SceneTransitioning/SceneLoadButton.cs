// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// Provides a way for scenes to be loaded when a button is clicked
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SceneLoadButton : MonoBehaviour
    {
        [Header("Button Settings")]
        [SerializeField, Tooltip("Makes sure the user can't load something multiple times")]
        private bool disableOnClick = true;

        [Header("Load Settings")]
        [SerializeField]
        private bool showLoadingScreen = false;

        [SerializeField, Tooltip("Unload all currently loaded scenes except for the initialization one")]
        private bool overwrite = true;

        [SerializeField]
        private SceneInfoSO[] scenesToLoad = null;

        [Header("Channel broadcasting on")]
        [SerializeField]
        private SceneLoadEventChannelSO channel = null;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(RequestSceneLoad);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(RequestSceneLoad);
        }

        private void RequestSceneLoad()
        {
            channel.RaiseRequest(scenesToLoad, showLoadingScreen, overwrite);

            if (disableOnClick)
            {
                button.interactable = false;
            }
        }
    }
}