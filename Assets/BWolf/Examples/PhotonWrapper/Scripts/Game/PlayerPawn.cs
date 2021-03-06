﻿using BWolf.Wrappers.PhotonSDK.Synchronization;
using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    /// <summary>Component for the player to drag and drop a pawn on the board</summary>
    public class PlayerPawn : MonoBehaviour, IDraggable
    {
        private PlacementPin collidingPin;
        private Vector3 startPosition;

        private MovableNetworkedObject movable;

        public bool IsDraggable
        {
            get { return movable.IsMine; }
        }

        private void Awake()
        {
            movable = GetComponent<MovableNetworkedObject>();
            GameBoardHandler.OnSetupFinished += StoreStartPosition;
        }

        private void OnDestroy()
        {
            GameBoardHandler.OnSetupFinished -= StoreStartPosition;
        }

        /// <summary>Called when this pawn is being dragged by the player it sets x and y positions according to given board position</summary>
        public void Drag(Vector3 boardPosition)
        {
            Vector3 position = transform.position;
            position.x = boardPosition.x;
            position.y = boardPosition.y;
            transform.position = position;
        }

        /// <summary>Called when this pawn is release by the player, if this pawn is still colliding with a pin, return to that pin, else resets to start position</summary>
        public void Release()
        {
            if (collidingPin != null)
            {
                transform.position = collidingPin.transform.position;
                collidingPin.SetHoldingPawn(this);
                TurnManager.FinishTurn(collidingPin.transform.GetSiblingIndex());
            }
            else
            {
                transform.position = startPosition;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!movable.IsMine) { return; }

            PlacementPin pin = other.GetComponentInParent<PlacementPin>();
            if (pin != null && !pin.HasPawn)
            {
                collidingPin = pin;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!movable.IsMine) { return; }

            PlacementPin pin = other.GetComponentInParent<PlacementPin>();
            if (pin != null && pin == collidingPin)
            {
                collidingPin = null;
            }
        }

        private void StoreStartPosition()
        {
            startPosition = transform.position;
        }
    }
}