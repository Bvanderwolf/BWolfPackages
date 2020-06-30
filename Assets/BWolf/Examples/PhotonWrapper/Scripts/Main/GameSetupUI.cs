using BWolf.Wrappers.PhotonSDK;
using BWolf.Wrappers.PhotonSDK.Handlers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper.Main
{
    /// <summary>Component class for setting up the game before starting the game</summary>
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

        /// <summary>Setsup player color picking and shares the first available color as this clients player color property</summary>
        private void SetupPlayerColorButton()
        {
            RefreshPlayerColorButtonListeners();

            playerColorPicker.UpdateAvailableColors();

            //set first available color from color picker as player color property
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerColorKey, playerColorPicker.FirstAvailableColor);
        }

        /// <summary>Refreshes player color butotn listeners by removing current from both and assigning one based on whether this client is the hos or not</summary>
        private void RefreshPlayerColorButtonListeners()
        {
            //clear up all listeners
            btnPlayerOneColor.onClick.RemoveListener(OnPlayerColorButtonClick);
            btnPlayerTwoColor.onClick.RemoveListener(OnPlayerColorButtonClick);

            //assign player color button based on who is the host
            playerColorButton = NetworkingService.IsHost ? btnPlayerOneColor : btnPlayerTwoColor;
            playerColorButton.onClick.AddListener(OnPlayerColorButtonClick);
        }

        /// <summary>Called when the player's player color button has been clicked to active the player color picker</summary>
        private void OnPlayerColorButtonClick()
        {
            playerColorPicker.gameObject.SetActive(true);
            ToggleInteractabilityOfColorButton();
        }

        /// <summary>Called when a color has been picked to share this new color as player property with other clients in the room</summary>
        private void OnColorPicked(Color col)
        {
            ToggleInteractabilityOfColorButton();
            NetworkingService.UpdateClientProperty(ClientHandler.PlayerColorKey, col);
        }

        /// <summary>Updates the color of the player color buttons based on whether the client is the host or not</summary>
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

        /// <summary>Toggles the interactablity of the player's player color button</summary>
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