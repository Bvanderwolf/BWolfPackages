using UnityEngine;

namespace BWolf.Utilities.SquadFormations.Selection
{
    /// <summary>Utility component for fake instancing the materials for the projector to make sure color changes are not applied to all materials</summary>
    public class DecalProjector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Material decalMaterial = null;

        [SerializeField]
        private Texture2D decalTexture = null;

        private Material materialInstance;
        private Projector projector;

        private void Awake()
        {
            projector = GetComponent<Projector>();
            materialInstance = new Material(decalMaterial);
            materialInstance.SetTexture("_Texture", decalTexture);
            projector.material = materialInstance;
        }
    }
}