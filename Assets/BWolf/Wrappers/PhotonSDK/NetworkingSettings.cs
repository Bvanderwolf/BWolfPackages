using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    [CreateAssetMenu(menuName = "NetworkingService/Settings")]
    public class NetworkingSettings : ScriptableObject
    {
        [SerializeField]
        private int serializationRate = 20;

        [SerializeField]
        private int sendRate = 40;

        [SerializeField]
        private bool synchronizeClientScenes = true;

        public int SerializationRate
        {
            get { return serializationRate; }
        }

        public int SendRate
        {
            get { return sendRate; }
        }

        public bool SynchronizeClientScenes
        {
            get { return synchronizeClientScenes; }
        }
    }
}