using UnityEngine;

namespace BWolf.Examples.AgentCommands
{
    public class PickupableObstacle : PickupableObject
    {
        [SerializeField]
        private float pickupDistance = 0.5f;

        public override float PickupDistance
        {
            get { return pickupDistance; }
        }

        public override bool IsPickedUp()
        {
            return true;
        }

        public override void StartPickup()
        {
            gameObject.SetActive(false);
        }

        public override void UndoPickup()
        {
            gameObject.SetActive(true);
        }
    }
}