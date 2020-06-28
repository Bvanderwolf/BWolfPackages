using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public interface IDraggable
    {
        void StartDrag();

        void Drag(Vector3 boardPosition);

        void Release();
    }
}