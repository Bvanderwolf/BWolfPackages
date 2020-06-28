using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    /// <summary>Component for interaction with player pawns to be placed on the board</summary>
    public class PlacementPin : MonoBehaviour
    {
        private PlayerPawn pawnHolding;

        /// <summary>Returns whether this placement pin holds a pawn or not</summary>
        public bool HasPawn
        {
            get { return pawnHolding != null; }
        }

        /// <summary>Stores the pawn as holding pawn</summary>
        public void SetHoldingPawn(PlayerPawn pawn)
        {
            pawnHolding = pawn;
        }

        private void OnTriggerEnter(Collider other)
        {
        }

        private void OnTriggerExit(Collider other)
        {
            //reset the pawn holding reference if the pawn being held has been taken out of trigger range
            PlayerPawn pawn = other.GetComponentInParent<PlayerPawn>();
            if (pawn != null && pawn == pawnHolding)
            {
                pawnHolding = null;
            }
        }
    }
}