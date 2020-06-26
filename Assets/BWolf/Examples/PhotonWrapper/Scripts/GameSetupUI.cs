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

        private Button playerColorButton = null;

        private void Awake()
        {
            playerColorPicker.OnColorPicked += OnColorPicked;
            playerColorPicker.OnCancel += ToggleInteractabilityOfColorButton;

            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.AddCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
            NetworkingService.AddClientPropertyUpdateListener(OnClientPropertyUpdate);
        }

        private void OnDestroy()
        {
            playerColorPicker.OnColorPicked -= OnColorPicked;
            playerColorPicker.OnCancel -= ToggleInteractabilityOfColorButton;

            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientJoined, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.ClientLeft, OnRoomUpdate);
            NetworkingService.RemoveCallbackListener(InRoomCallbackEvent.HostChanged, OnRoomUpdate);
            NetworkingService.RemoveClientPropertyUpdateListener(OnClientPropertyUpdate);
        }

        private void SetupPlayerColorButton()
        {
            RefreshPlayerColorButtonListeners();

            playerColorPicker.UpdateAvailableColors();

            //set first available color from color picker as player color property
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerColorKey, playerColorPicker.FirstAvailableColor);
        }

        private void RefreshPlayerColorButtonListeners()
        {
            //clear up all listeners
            btnPlayerOneColor.onClick.RemoveListener(OnPlayerColorButtonClick);
            btnPlayerTwoColor.onClick.RemoveListener(OnPlayerColorButtonClick);

            //assign player color button based on who is the host
            playerColorButton = NetworkingService.IsHost ? btnPlayerOneColor : btnPlayerTwoColor;
            playerColorButton.onClick.AddListener(OnPlayerColorButtonClick);
        }

        private void OnPlayerColorButtonClick()
        {
            playerColorPicker.gameObject.SetActive(true);
            ToggleInteractabilityOfColorButton();
        }

        private void OnColorPicked(Color col)
        {
            ToggleInteractabilityOfColorButton();
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerColorKey, col);
        }

        private void UpdatePlayerColorButtons()
        {
            foreach (Client client in NetworkingService.ClientsInRoom.Values)
            {
                Color color = (Color)client.Properties[ClientHandler.PlayerColorKey];
                if (client.IsHost)
                {
                    btnPlayerOneColor.targetGraphic.color = color;
                    btnPlayerOneColor.colors = playerColorPicker.CreateColorBlock(color);
                }
                else
                {
                    btnPlayerTwoColor.targetGraphic.color = color;
                    btnPlayerTwoColor.colors = playerColorPicker.CreateColorBlock(color);
                }
            }
        }

        private void ToggleInteractabilityOfColorButton()
        {
            playerColorButton.interactable = !playerColorButton.interactable;
        }

        /// <summary>Called when having joined a room, it updates the ui elements accordingly</summary>
        private void OnJoinedRoom(string message)
        {
            SetupPlayerColorButton();
            UpdateUIElements();
        }

        /// <summary>Called when an update inside the room happens, it updates the ui elements accordingly</summary>
        private void OnRoomUpdate(Client client)
        {
            RefreshPlayerColorButtonListeners();
            playerColorPicker.UpdateAvailableColors();
            UpdateUIElements();
        }

        private void OnClientPropertyUpdate(Client client, Dictionary<string, object> properties)
        {
            if (properties.ContainsKey(ClientHandler.PlayerColorKey))
            {
                UpdatePlayerColorButtons();
                playerColorPicker.UpdateAvailableColors();
            }
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