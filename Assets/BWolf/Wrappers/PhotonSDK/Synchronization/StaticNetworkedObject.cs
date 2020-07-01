using BWolf.Wrappers.PhotonSDK.Handlers;

namespace BWolf.Wrappers.PhotonSDK.Synchronization
{
    /// <summary>Used by static objects in scene to be manageable</summary>
    public class StaticNetworkedObject : NetworkedObject
    {
        public const int BaseIdNumber = 21 * 1000; //this number is based on 20 max people in one room * 1000 photon id's per individual + 1000 ids for scene objects

        private int viewId = -1;

        public override bool IsMine
        {
            get { return NetworkingService.IsHost; }
        }

        private void Awake()
        {
            transform.parent = ResourceHandler.GetPoolParent(false);
        }

        /// <summary>the identifying nubmer for this object. id's start at 1000 to avoid conflict with photon view id's.</summary>
        public override int ViewId
        {
            get { return viewId; }
        }

        /// <summary>Sets the id of this static object view incorperating the base id number</summary>
        public void SetViewId(int id)
        {
            if (viewId == -1)
            {
                viewId = BaseIdNumber + id;
            }
        }
    }
}