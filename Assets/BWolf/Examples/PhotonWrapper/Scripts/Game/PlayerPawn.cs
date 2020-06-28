using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class PlayerPawn : MonoBehaviour, IDraggable
    {
        public void Drag(Vector3 boardPosition)
        {
            Vector3 position = transform.position;
            position.x = boardPosition.x;
            position.y = boardPosition.y;
            transform.position = position;
        }

        public void Release()
        {
            print("releasing");
        }
    }
}