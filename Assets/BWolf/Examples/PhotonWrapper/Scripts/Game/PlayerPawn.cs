using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class PlayerPawn : MonoBehaviour, IDraggable
    {
        private PlacementPin collidingPin;
        private Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.position;
        }

        public void StartDrag()
        {
        }

        public void Drag(Vector3 boardPosition)
        {
            Vector3 position = transform.position;
            position.x = boardPosition.x;
            position.y = boardPosition.y;
            transform.position = position;
        }

        public void Release()
        {
            if (collidingPin != null)
            {
                transform.position = collidingPin.transform.position;
                collidingPin.AttachPlayerPawn(this);
            }
            else
            {
                transform.position = startPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PlacementPin pin = other.GetComponentInParent<PlacementPin>();
            if (pin != null && !pin.HasPawn)
            {
                collidingPin = pin;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlacementPin pin = other.GetComponentInParent<PlacementPin>();
            if (pin != null && pin == collidingPin)
            {
                collidingPin = null;
            }
        }
    }
}