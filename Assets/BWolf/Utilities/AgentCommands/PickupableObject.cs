using UnityEngine;

namespace BWolf.Utilities.AgentCommands
{
    public abstract class PickupableObject : MonoBehaviour, IPickupable
    {
        public Vector3 GetPickupPosition(Vector3 fromPosition)
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            float x = bounds.extents.x;
            float z = bounds.extents.z;
            float max = (x >= z ? x : z) + PickupDistance;
            Vector3 position = transform.position;
            return position + ((fromPosition - position).normalized * max);
        }

        public abstract float PickupDistance { get; }

        public abstract void StartPickup();

        public abstract void UndoPickup();

        public abstract bool IsPickedUp();
    }
}