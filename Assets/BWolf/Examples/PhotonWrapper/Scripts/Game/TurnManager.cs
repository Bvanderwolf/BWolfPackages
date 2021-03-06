﻿using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.DataContainers;
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

        public const string NameOfTurnFinishedEvent = "TurnFinished";

        private void Awake()
        {
            defaultOutlineWidth = playerOneHead.outlineWidth;
            playerOneHead.outlineWidth = playerTurnOutlineWidth;

            GameBoardHandler.OnSetupFinished += OnSetupFinished;
            NetworkingService.AddGameEventListener(NameOfTurnFinishedEvent, OnTurnFinished);
        }

        private void OnDestroy()
        {
            GameBoardHandler.OnSetupFinished -= OnSetupFinished;
            NetworkingService.RemoveGameEventListener(NameOfTurnFinishedEvent, OnTurnFinished);
        }

        private void OnSetupFinished()
        {
            if (!NetworkingService.IsHost)
            {
                FinishTurn(-1);
            }
        }

        private void OnTurnFinished(object obj)
        {
            TurnFinishedInfo info = (TurnFinishedInfo)obj;
            Client finishedClient = NetworkingService.ClientsInRoom[info.ActorNrOfFinishedClient];
            HasTurn = !finishedClient.IsLocal;
            playerOneHead.outlineWidth = finishedClient.IsHost ? defaultOutlineWidth : playerTurnOutlineWidth;
            playerTwoHead.outlineWidth = !finishedClient.IsHost ? defaultOutlineWidth : playerTurnOutlineWidth;
        }

        public static void FinishTurn(int gridIndex)
        {
            TurnFinishedInfo info = new TurnFinishedInfo(NetworkingService.LocalClient.ActorNumber, gridIndex);
            NetworkingService.RaiseGameEvent(NameOfTurnFinishedEvent, info, EventReceivers.All);
        }
    }
}