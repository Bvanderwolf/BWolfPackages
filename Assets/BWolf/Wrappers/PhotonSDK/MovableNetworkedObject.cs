using BWolf.Wrappers.PhotonSDK.Handlers;
using Photon.Pun;
using UnityEngine;

namespace BWolf.Wrappers.PhotonSDK
{
    /// <summary>Handles streaming of gameobject transform properties like position, rotation and scale</summary>
    [RequireComponent(typeof(PhotonView))]
    public class MovableNetworkedObject : NetworkedObject, IPunObservable
    {
        [Header("Settings")]
        [SerializeField]
        private bool synchronizePosition = false;

        [SerializeField]
        private bool synchronizeRotation = false;

        [SerializeField]
        private bool synchronizeScale = false;

        private float distance;
        private float angle;
        private Vector3 direction;
        private Vector3 networkPosition;
        private Vector3 storedPosition;

        private Quaternion networkRotation;

        private bool firstTake;

        public override bool IsMine
        {
            get { return View.IsMine; }
        }

        public bool IsSceneObject
        {
            get { return View.IsSceneView; }
        }

        public override int ViewId
        {
            get { return View.ViewID; }
        }

        public int OwnerActorNr
        {
            get { return View.OwnerActorNr; }
        }

        public PhotonView View { get; private set; }

        public void Awake()
        {
            View = GetComponent<PhotonView>();

            transform.parent = ResourceHandler.GetPoolParent(true);
        }

        private void OnEnable()
        {
            firstTake = true;

            storedPosition = transform.position;
            networkPosition = transform.position;

            networkRotation = Quaternion.identity;
        }

        public void Update()
        {
            //only synchronize transform if options are set and this isn't our object
            bool canSynchronize = synchronizePosition || synchronizeRotation || synchronizeScale;
            if (canSynchronize && !View.IsMine)
            {
                float tickTime = 1.0f / PhotonNetwork.SerializationRate;
                transform.position = Vector3.MoveTowards(transform.position, networkPosition, distance * tickTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, angle * tickTime);
            }
        }

        /// <summary>Used for storing the synchronized transform values gained from the server so we can linearly interpolate towards these during game frames</summary>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (synchronizePosition)
                {
                    direction = transform.position - storedPosition;
                    storedPosition = transform.position;

                    stream.SendNext(transform.position);
                    stream.SendNext(direction);
                }

                if (synchronizeRotation)
                {
                    stream.SendNext(transform.rotation);
                }

                if (synchronizeScale)
                {
                    stream.SendNext(transform.localScale);
                }
            }
            else
            {
                if (synchronizePosition)
                {
                    networkPosition = (Vector3)stream.ReceiveNext();
                    direction = (Vector3)stream.ReceiveNext();

                    if (firstTake)
                    {
                        transform.position = networkPosition;
                        distance = 0f;
                    }
                    else
                    {
                        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                        networkPosition += direction * lag;
                        distance = Vector3.Distance(transform.position, networkPosition);
                    }
                }

                if (synchronizeRotation)
                {
                    networkRotation = (Quaternion)stream.ReceiveNext();

                    if (firstTake)
                    {
                        angle = 0f;
                        transform.rotation = networkRotation;
                    }
                    else
                    {
                        angle = Quaternion.Angle(transform.rotation, networkRotation);
                    }
                }

                if (synchronizeScale)
                {
                    transform.localScale = (Vector3)stream.ReceiveNext();
                }

                if (firstTake)
                {
                    firstTake = false;
                }
            }
        }
    }
}