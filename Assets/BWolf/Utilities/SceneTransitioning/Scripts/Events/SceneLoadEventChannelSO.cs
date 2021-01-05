// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;
using System;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// A scriptable object serving as a channel for scene load events to be broadcasted
    /// </summary>
    [CreateAssetMenu(fileName = "LoadEventChannel", menuName = "SO Event Channels/SceneLoading")]
    public class SceneLoadEventChannelSO : ScriptableObject
    {
        public Action<SceneInfoSO[], bool, bool> OnRequestRaised;

        public void RaiseRequest(SceneInfoSO[] scenes, bool showLoadingScreen, bool overwrite)
        {
            if (OnRequestRaised != null)
            {
                OnRequestRaised(scenes, showLoadingScreen, overwrite);
            }
            else
            {
                Debug.LogWarning("A scene load was requested, but nobody picked it up. " +
               "Check why there is no SceneLoader already loaded, " +
               "and make sure it's listening on this Scene Load Event channel.");
            }
        }
    }
}