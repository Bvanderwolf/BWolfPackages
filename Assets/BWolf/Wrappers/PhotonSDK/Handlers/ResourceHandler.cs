using Photon.Pun;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Wrappers.PhotonSDK.Handlers
{
    public class ResourceHandler : IPunPrefabPool
    {
        private static Dictionary<int, GameObject> staticObjectDict = new Dictionary<int, GameObject>();

        private static Transform PoolParent;
        private static Transform movingObjectParent;
        private static Transform staticObjectParent;

        private Dictionary<string, ConcurrentQueue<GameObject>> resourcePoolDict = new Dictionary<string, ConcurrentQueue<GameObject>>();

        private NetworkingSettings settings;

        public ResourceHandler(NetworkingSettings settings)
        {
            this.settings = settings;

            VerifyMovingObjects();
            VerifyStaticObjects();
            CreatePoolParents();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            PhotonNetwork.PrefabPool = this;
        }

        private void CreatePoolParents()
        {
            if (PoolParent != null) { GameObject.Destroy(PoolParent.gameObject); }
            PoolParent = new GameObject("PoolParent").transform;

            movingObjectParent = new GameObject("MovingObjects").transform;
            movingObjectParent.parent = PoolParent;

            staticObjectParent = new GameObject("StaticObjects").transform;
            staticObjectParent.parent = PoolParent;

            GameObject.DontDestroyOnLoad(PoolParent.gameObject);
        }

        /// <summary>Returns a transform based on given movingObject flag</summary>
        public static Transform GetPoolParent(bool movingObject)
        {
            return movingObject ? movingObjectParent : staticObjectParent;
        }

        private void VerifyStaticObjects()
        {
            foreach (GameObject prefab in settings.StaticNetworkedObjects)
            {
                if (prefab.GetComponent<StaticNetworkedObject>() == null)
                {
                    Debug.LogError("prefab with name " + prefab.name + " doesn't have a NetworkedStaticObject component. This is necessary");
                    continue;
                }

                resourcePoolDict.Add(prefab.name, new ConcurrentQueue<GameObject>());
            }
        }

        /// <summary>Verifies moving objects to be pooled in the moving objects array</summary>
        private void VerifyMovingObjects()
        {
            foreach (GameObject prefab in settings.MovableNetworkedObjects)
            {
                if (prefab.activeSelf)
                {
                    //to comply with photon standards the resource needs to be inactive
                    Debug.LogWarning("prefab with name " + prefab.name + "was active which is not complient with photon instantiation rules");
                    prefab.SetActive(false);
                }
                PhotonView photonView = prefab.GetComponent<PhotonView>();
                if (photonView == null || photonView.OwnershipTransfer != OwnershipOption.Takeover)
                {
                    Debug.LogError("prefab with photonview " + photonView + " is either null or doesn't have the takeover ownership option enabled");
                    continue;
                }
                if (prefab.GetComponent<MovableNetworkedObject>() == null)
                {
                    Debug.LogError("prefab with name " + prefab.name + " doesn't have a MovableNetworkedObject component. This is necessary");
                    continue;
                }

                resourcePoolDict.Add(prefab.name, new ConcurrentQueue<GameObject>());
            }
        }

        private void OnSceneUnloaded(Scene scene)
        {
            //empty queue for every prefab
            GameObject empty = null;
            foreach (var resource in resourcePoolDict)
            {
                while (resource.Value.TryDequeue(out empty)) { }
            }

            staticObjectDict.Clear();
        }

        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            return null;
        }

        public void Destroy(GameObject gameObject)
        {
        }
    }
}