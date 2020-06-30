using System;
using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.Editables;
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

        public static bool HasTurn { get; private set; }

        private float defaultOutlineWidth;

        private void Awake()
        {
            defaultOutlineWidth = playerOneHead.outlineWidth;
            playerOneHead.outlineWidth = playerTurnOutlineWidth;

            GameBoardHandler.OnSetupFinished += OnSetupFinished;
            NetworkingService.AddGameEventListener(GameEvent.TurnFinished, OnTurnFinished);
        }

        private void OnDestroy()
        {
            GameBoardHandler.OnSetupFinished -= OnSetupFinished;
            NetworkingService.RemoveGameEventListener(GameEvent.TurnFinished, OnTurnFinished);
        }

        private void OnSetupFinished()
        {
            if (!NetworkingService.IsHost)
            {
                FinishTurn();
            }
        }

        private void OnTurnFinished(object obj)
        {
            int actorNr = (int)obj;
            Client finishedClient = NetworkingService.ClientsInRoom[actorNr];
            HasTurn = !finishedClient.IsLocal;
            playerOneHead.outlineWidth = finishedClient.IsHost ? defaultOutlineWidth : playerTurnOutlineWidth;
            playerTwoHead.outlineWidth = !finishedClient.IsHost ? defaultOutlineWidth : playerTurnOutlineWidth;
        }

        public static void FinishTurn()
        {
            NetworkingService.RaiseGameEvent(GameEvent.TurnFinished, NetworkingService.LocalClient.ActorNumber, EventReceivers.All);
        }
    }
}