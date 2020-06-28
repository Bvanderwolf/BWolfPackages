using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public interface IDraggable
    {
        void Drag(Vector3 boardPosition);

        void Release();
    }
}