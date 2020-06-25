using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class GameSetupUI : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private Button btnStart = null;

        [SerializeField]
        private PlayerColorPicker playerColorPicker = null;

        [Header("PlayerOne")]
        [SerializeField]
        private Text txtPlayerOne = null;

        [SerializeField]
        private Button btnPlayerOneColor = null;

        [Header("PlayerTwo")]
        [SerializeField]
        private Text txtPlayerTwo = null;

        [SerializeField]
        private Button btnPlayerTwoColor = null;

        private void Awake()
        {
            btnPlayerOneColor.onClick.AddListener(OnPlayerOneColorButtonClick);
            btnPlayerTwoColor.onClick.AddListener(OnPlayerTwoColorButtonClick);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
        }

        private void OnDestroy()
        {
            btnPlayerOneColor.onClick.RemoveListener(OnPlayerOneColorButtonClick);
            btnPlayerTwoColor.onClick.RemoveListener(OnPlayerTwoColorButtonClick);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
        }

        private void OnPlayerOneColorButtonClick()
        {
            playerColorPicker.gameObject.SetActive(true);
            playerColorPicker.SetColorButtonToModify(btnPlayerOneColor);
        }

        private void OnPlayerTwoColorButtonClick()
        {
            playerColorPicker.gameObject.SetActive(true);
            playerColorPicker.SetColorButtonToModify(btnPlayerTwoColor);
        }

        /// <summary>Called when having joined a room, it updates the ui elements accordingly</summary>
        private void OnJoinedRoom(string message)
        {
            UpdateUIElements();
        }

        /// <summary>Called when an update inside the room happens, it updates the ui elements accordingly</summary>
        private void OnRoomUpdate(Client client)
        {
            UpdateUIElements();
        }

        /// <summary>Updates player one text and player two text based on who is the host and sets interactive state of start button</summary>
        private void UpdateUIElements()
        {
            txtPlayerOne.text = string.Empty;
            txtPlayerTwo.text = string.Empty;

            //since there can only be 2 players in a room this foreach loop is feasable
            Dictionary<int, Client> clients = NetworkingService.ClientsInRoom;
            foreach (var client in clients)
            {
                if (client.Key == NetworkingService.ActorNumberOfHost)
                {
                    txtPlayerOne.text = client.Value.Nickname;
                }
                else
                {
                    txtPlayerTwo.text = client.Value.Nickname;
                }
            }

            //set start butotn interactable state based on whether room is filled
            btnStart.interactable = clients.Count == RoomItemsUI.DemoGameMaxPlayers && NetworkingService.IsHost;
        }
    }
}