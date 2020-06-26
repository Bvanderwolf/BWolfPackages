using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class PlayerLineups : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private MeshRenderer backdropRenderer = null;

        [SerializeField]
        private Transform playerOneLineup = null;

        [SerializeField]
        private Transform playerTwoLineup = null;
    }
}