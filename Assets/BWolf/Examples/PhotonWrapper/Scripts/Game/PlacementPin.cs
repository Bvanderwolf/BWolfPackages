using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class PlacementPin : MonoBehaviour
    {
        private PlayerPawn pawnHolding;

        public bool HasPawn
        {
            get { return pawnHolding != null; }
        }

        public void AttachPlayerPawn(PlayerPawn pawn)
        {
            pawnHolding = pawn;
        }

        private void OnTriggerEnter(Collider other)
        {
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerPawn pawn = other.GetComponentInParent<PlayerPawn>();
            print(pawn);
            if (pawn != null && pawn == pawnHolding)
            {
                print("test");
                pawnHolding = null;
            }
        }
    }
}