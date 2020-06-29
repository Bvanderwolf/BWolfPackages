using BWolf.Wrappers.PhotonSDK.Serialization;
using Photon.Pun;
using Photon.Realtime;
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

        ~ResourceHandler()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
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

        /// <summary>Called when a client wants the host to spawn a scene object, it spawns a sceneobject based on given information</summary>
        private void OnSceneObjectSpawn(object content)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ObjectSpawnInfo info = (ObjectSpawnInfo)content;
                PhotonNetwork.InstantiateSceneObject(info.PrefabId, info.Position, info.Rotation);
            }
        }

        /// <summary>Called when an event has been raised to spawn a static object, it will create one using the given information</summary>
        private void OnStaticObjectSpawn(object content)
        {
            ObjectSpawnInfo info = (ObjectSpawnInfo)content;

            //try getting a new resource from the poolad game objects stored in the queue
            GameObject instance = null;
            ConcurrentQueue<GameObject> pooledGameObjects = resourcePoolDict[info.PrefabId];
            if (pooledGameObjects.TryDequeue(out instance))
            {
                instance.transform.position = info.Position;
                instance.transform.rotation = info.Rotation;
            }
            else
            {
                //if there are no avaialbe pooled game objects, the used resource for instantiation will be the original prefab
                GameObject res = null;
                foreach (GameObject prefab in settings.StaticNetworkedObjects)
                {
                    if (prefab.name == info.PrefabId)
                    {
                        res = prefab;
                        break;
                    }
                }

                instance = GameObject.Instantiate(res, info.Position, info.Rotation);
                instance.name = res.name;

                //set static object view id and add it to stored object dictionary searches
                StaticNetworkedObject obj = instance.GetComponent<StaticNetworkedObject>();
                obj.SetViewId(staticObjectDict.Count);
                staticObjectDict.Add(obj.ViewId, instance);
            }

            instance.SetActive(true);
        }

        /// <summary>Called when an event has been raised to destroy a static object, it will reset the gameobject and return it to the pool</summary>
        private void OnStaticObjectDestroy(object content)
        {
            GameObject go = staticObjectDict[(int)content];

            //destroy gameobject normally and exit if player destroys object when not in a room
            if (PhotonNetwork.NetworkClientState != ClientState.Joined)
            {
                GameObject.Destroy(go);
                return;
            }

            if (resourcePoolDict.ContainsKey(go.name))
            {
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.SetActive(false);
                resourcePoolDict[go.name].Enqueue(go);
            }
            else
            {
                Debug.LogWarning("Failed Destroying static networked gameobject with name " + go.name + " because it wasn't in the resource dictionary");
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

        /// <summary>Returns a transform based on given movingObject flag</summary>
        public static Transform GetPoolParent(bool movingObject)
        {
            return movingObject ? movingObjectParent : staticObjectParent;
        }

        /// <summary>Will either return an instantiated or pooled object</summary>
        public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
        {
            //exit with error if the prefabId isn't a key in the resourcePool
            if (!resourcePoolDict.ContainsKey(prefabId))
            {
                Debug.LogError("Failed instantiating object with name " + prefabId + ". Make sure it matches the original prefab name");
                return null;
            }

            //try getting a new resource from the pooled game objects stored in the queue
            GameObject instance = null;
            ConcurrentQueue<GameObject> pooledGameObjects = resourcePoolDict[prefabId];
            if (pooledGameObjects.TryDequeue(out instance))
            {
                instance.transform.position = position;
                instance.transform.rotation = rotation;
            }
            else
            {
                //if there are no avaialbe pooled game objects, the used resource for instantiation will be the original prefab
                GameObject res = null;
                foreach (GameObject prefab in settings.MovableNetworkedObjects)
                {
                    if (prefab.name == prefabId)
                    {
                        res = prefab;
                        break;
                    }
                }

                instance = GameObject.Instantiate(res, position, rotation);
                instance.name = res.name;
            }

            return instance;
        }

        /// <summary>Will either destroy or pool the given object depending on state of player in the room</summary>
        public void Destroy(GameObject go)
        {
            //destroy gameobject normally and exit if player destroys object when not in a room
            if (PhotonNetwork.NetworkClientState != ClientState.Joined)
            {
                GameObject.Destroy(go);
                return;
            }

            if (resourcePoolDict.ContainsKey(go.name))
            {
                //if the gameobject to be destroyed is in the resource dictionary it can be reset and enqueued for reusing
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.SetActive(false);
                //transfer ownership to scene while this gameobject is inactive
                go.GetComponent<PhotonView>().TransferOwnership(0);
                resourcePoolDict[go.name].Enqueue(go);
            }
            else
            {
                Debug.LogWarning("Failed Destroying gameobject with name " + go.name + " because it wasn't in the resource dictionary");
            }
        }
    }
}