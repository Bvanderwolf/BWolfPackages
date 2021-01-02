using UnityEngine;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// A scriptable object serving as a channel for scene load events to be broadcasted
    /// </summary>
    [CreateAssetMenu(fileName = "LoadEventChannel", menuName = "SO Event Channels/SceneLoading")]
    public class SceneLoadEventChannelSO : ScriptableObject
    {
        public System.Action<SceneInfoSO[], bool, bool> OnRequestRaised;

        public void RaiseRequest(SceneInfoSO[] scenes, bool showLoadingScreen, bool overwrite)
        {
            OnRequestRaised?.Invoke(scenes, showLoadingScreen, overwrite);
        }
    }
}