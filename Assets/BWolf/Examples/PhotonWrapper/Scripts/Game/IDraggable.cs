﻿using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    /// <summary>Interface used from dragging pawns on the game board</summary>
    public interface IDraggable
    {
        /// <summary>Is this draggable object draggable by this client or not</summary>
        bool IsDraggable { get; }

        /// <summary>drags the draggable based on the board position on which is clicked</summary>
        void Drag(Vector3 boardPosition);

        /// <summary>Releases draggable</summary>
        void Release();
    }
}