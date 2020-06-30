using TMPro;
using UnityEngine;

namespace BWolf.Examples.PhotonWrapper.Game
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text playerOneHead = null;

        [SerializeField]
        private TMP_Text playerTwoHead = null;

        [SerializeField]
        private float playerTurnOutlineWidth = 0.25f;

        private float defaultOutlineWidth;

        private void Awake()
        {
            defaultOutlineWidth = playerOneHead.outlineWidth;
            playerOneHead.outlineWidth = playerTurnOutlineWidth;
        }
    }
}