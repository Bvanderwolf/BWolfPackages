using UnityEngine;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// A scriptable object serving as a channel for ASync operation events to be broadcasted
    /// </summary>
    [CreateAssetMenu(fileName = "ASyncOperationChannel", menuName = "SO Event Channels/ASync Operations")]
    public class ASyncOperationsEventChannelSO : ScriptableObject
    {
        public System.Action<AsyncOperation[]> OnRequestRaised;

        public void RaiseRequest(AsyncOperation[] loadOperations)
        {
            OnRequestRaised?.Invoke(loadOperations);
        }
    }
}