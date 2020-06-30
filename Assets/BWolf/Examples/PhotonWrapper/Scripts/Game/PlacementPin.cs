using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    /// <summary>Component for interaction with player pawns to be placed on the board</summary>
    public class PlacementPin : MonoBehaviour
    {
        [SerializeField]
        private Color emissionColor = Color.black;

        [SerializeField]
        private PlayerPawn pawnHolding;

        private BoxCollider boxCollider;
        private Material[] materials;
        private Vector3 colliderSize;

        private readonly Color defaultEmissionColor = Color.black;

        /// <summary>Returns whether this placement pin holds a pawn or not</summary>
        public bool HasPawn
        {
            get { return pawnHolding != null; }
        }

        private void Awake()
        {
            materials = GetComponentInChildren<Renderer>().materials;
            boxCollider = GetComponent<BoxCollider>();
            colliderSize = boxCollider.size;

            foreach (Material mat in materials)
            {
                mat.EnableKeyword("_EMISSION");
            }
        }

        /// <summary>Stores the pawn as holding pawn</summary>
        public void SetHoldingPawn(PlayerPawn pawn)
        {
            pawnHolding = pawn;
            boxCollider.size = colliderSize * 0.5f;
            foreach (Material mat in materials)
            {
                mat.SetColor("_EmissionColor", defaultEmissionColor);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Pawn")) { return; }

            PlayerPawn pawn = other.GetComponentInParent<PlayerPawn>();
            if (pawn != null && pawn.IsDraggable && pawnHolding == null)
            {
                foreach (Material mat in materials)
                {
                    mat.SetColor("_EmissionColor", emissionColor);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Pawn")) { return; }

            //reset the pawn holding reference if the pawn being held has been taken out of trigger range
            PlayerPawn pawn = other.GetComponentInParent<PlayerPawn>();
            if (pawn != null && pawn.IsDraggable)
            {
                foreach (Material mat in materials)
                {
                    mat.SetColor("_EmissionColor", defaultEmissionColor);
                }

                if (pawn == pawnHolding)
                {
                    boxCollider.size = colliderSize;
                    pawnHolding = null;
                }
            }
        }
    }
}