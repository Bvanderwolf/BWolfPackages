using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    [CreateAssetMenu(menuName = "NetworkingService/Settings")]
    public class NetworkingSettings : ScriptableObject
    {
        [Header("Speed")]
        [SerializeField]
        private int serializationRate = 20;

        [SerializeField]
        private int sendRate = 40;

        [Header("SceneLoading")]
        [SerializeField]
        private bool synchronizeClientScenes = true;

        [Header("ObjectManagement")]
        [SerializeField]
        private GameObject[] movableNetworkedObjects = null;

        [SerializeField]
        private GameObject[] staticNetworkedObjects = null;

        /// <summary>Defines how many times per second streaming is done (tick time = 1/SerializationRate)</summary>
        public int SerializationRate
        {
            get { return serializationRate; }
        }

        /// <summary>Defines how many times per second the networking sdk should send a package. If you change this, do not forget to also change 'SerializationRate'.</summary>
        public int SendRate
        {
            get { return sendRate; }
        }

        /// <summary>If set to true, makes sure that when the host calls the networking service's loadlevel function, other clients in the room will also load this scene</summary>
        public bool SynchronizeClientScenes
        {
            get { return synchronizeClientScenes; }
        }

        public GameObject[] StaticNetworkedObjects
        {
            get { return staticNetworkedObjects; }
        }

        public GameObject[] MovableNetworkedObjects
        {
            get { return movableNetworkedObjects; }
        }

        /// <summary>Returns whether given prefabId corresponds to a static object</summary>
        public bool IsStaticObject(string prefabId)
        {
            for (int i = 0; i < staticNetworkedObjects.Length; i++)
            {
                if (staticNetworkedObjects[i].name == prefabId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}