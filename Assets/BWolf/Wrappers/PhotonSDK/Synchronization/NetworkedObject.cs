using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK.Synchronization
{
    /// <summary>Base class for moving and static object components</summary>
    [DisallowMultipleComponent]
    public abstract class NetworkedObject : MonoBehaviour
    {
        public abstract int ViewId { get; }

        public abstract bool IsMine { get; }
    }
}