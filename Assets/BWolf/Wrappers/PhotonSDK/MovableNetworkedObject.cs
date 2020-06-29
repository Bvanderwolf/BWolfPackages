using BWolf.Wrappers.PhotonSDK.Handlers;
using Photon.Pun;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>Handles streaming of gameobject transform properties like position, rotation and scale</summary>
    [RequireComponent(typeof(PhotonView))]
    public class MovableNetworkedObject : NetworkedObject
    {
        public override bool IsMine
        {
            get { return view.IsMine; }
        }

        public bool IsSceneObject
        {
            get { return view.IsSceneView; }
        }

        public override int ViewId
        {
            get { return view.ViewID; }
        }

        public int OwnerActorNr
        {
            get { return view.OwnerActorNr; }
        }

        private PhotonView view;

        public void Awake()
        {
            view = GetComponent<PhotonView>();

            transform.parent = ResourceHandler.GetPoolParent(true);
        }
    }
}