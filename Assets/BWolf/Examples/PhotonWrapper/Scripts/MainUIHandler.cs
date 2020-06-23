﻿using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    using System;
    using ListItem = ListItemsUI.ListItem;

    public class MainUIHandler : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField]
        private Text txtConnectionState = null;

        [SerializeField]
        private CanvasGroup[] canvasGroups = null;

        [SerializeField]
        private Button btnMultiplayer = null;

        [SerializeField]
        private Button btnOffline = null;

        [SerializeField]
        private Button btnSettings = null;

        [Header("Lobby")]
        [SerializeField]
        private LobbyItemsUI lobbyListItems = null;

        [SerializeField]
        private Button btnJoinLobby = null;

        [Header("Room")]
        [SerializeField]
        private RoomItemsUI roomListItems = null;

        [SerializeField]
        private Button btnJoinRoom = null;

        private Dictionary<string, CanvasGroup> canvasGroupDictionary = new Dictionary<string, CanvasGroup>();

        private void Awake()
        {
            //add listeners
            btnMultiplayer.onClick.AddListener(OnMultiplayerButtonClick);
            btnOffline.onClick.AddListener(OnOfflineButtonClick);
            btnSettings.onClick.AddListener(OnSettingsButtonClick);
            lobbyListItems.AddListener(OnLobbyItemSelect);
            roomListItems.AddListener(OnRoomItemSelect);

            //create dictionary entries using group name as key
            foreach (CanvasGroup group in canvasGroups)
            {
                canvasGroupDictionary.Add(group.name, group);
            }

            //add listeners for callbacks
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedLobby, OnJoinedLobby);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.LeftRoom, OnLeftRoom);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.LeftLobby, OnLeftLobby);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.CreatedRoom, OnCreatedRoom);

            //change group focus to menu buttons
            ChangeGroupFocus("MenuButtons");
        }

        private void Update()
        {
            txtConnectionState.text = string.Format("ConnectionState: {0}", NetworkingService.ConnectionState);
        }

        private void OnDestroy()
        {
            btnMultiplayer.onClick.RemoveListener(OnMultiplayerButtonClick);
            btnOffline.onClick.RemoveListener(OnOfflineButtonClick);
            btnSettings.onClick.RemoveListener(OnSettingsButtonClick);
            lobbyListItems.RemoveListener(OnLobbyItemSelect);
            roomListItems.RemoveListener(OnRoomItemSelect);

            //remove listeners on destroy
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedLobby, OnJoinedLobby);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedRoom, OnJoinedRoom);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.LeftRoom, OnLeftRoom);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.LeftLobby, OnLeftLobby);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.CreatedRoom, OnCreatedRoom);
        }

        /// <summary>Uses canvas groups to only show the given group name in the ui</summary>
        private void ChangeGroupFocus(string newFocusedGroupName)
        {
            if (canvasGroupDictionary.ContainsKey(newFocusedGroupName))
            {
                foreach (var groupEntry in canvasGroupDictionary)
                {
                    bool focused = groupEntry.Key == newFocusedGroupName;
                    groupEntry.Value.alpha = focused ? 1 : 0;
                    groupEntry.Value.blocksRaycasts = focused;
                    groupEntry.Value.interactable = focused;
                }
            }
        }

        /// <summary>Starts connection using the default settings if no connection is already established</summary>
        private void OnMultiplayerButtonClick()
        {
            if (!NetworkingService.IsConnected)
            {
                NetworkingService.ConnectWithDefaultSettings(() =>
                {
                    SetLobbyListFocus();
                });
            }
            else
            {
                SetLobbyListFocus();
            }
        }

        /// <summary>Starts offline mode and switches to offline mode buttons</summary>
        private void OnOfflineButtonClick()
        {
            if (!NetworkingService.IsConnected)
            {
                ChangeGroupFocus("OfflineButtons");
                NetworkingService.SetOffline(true);
            }
            else
            {
                NetworkingService.Disconnect(() =>
                {
                    ChangeGroupFocus("OfflineButtons");
                    NetworkingService.SetOffline(true);
                });
            }
        }

        private void OnSettingsButtonClick()
        {
            ChangeGroupFocus("Settings");
        }

        // <summary>Called when disconnected, changes to main menu buttons</summary>
        private void OnDisconnected(string cause)
        {
            ChangeGroupFocus("MenuButtons");
        }

        // <summary>Called when having joined a lobby with rooms, changes to room list focus</summary>
        private void OnJoinedLobby(string message)
        {
            SetRoomListFocus();
        }

        // <summary>Called when having left a lobby with rooms, changes to lobby list focus</summary>
        private void OnLeftLobby(string message)
        {
            SetLobbyListFocus();
        }

        // <summary>Called when having created a room, changes to players list focus</summary>
        private void OnCreatedRoom(string message)
        {
            ChangeGroupFocus("Players");
        }

        // <summary>Called when having joined a room, changes to players list focus/summary>
        private void OnJoinedRoom(string message)
        {
            ChangeGroupFocus("Players");
        }

        /// <summary>Called when having left a room, changes to room list focus</summary>
        private void OnLeftRoom(string message)
        {
            ChangeGroupFocus("Rooms");
        }

        // <summary>Called when a lobby item has been selected or not making the join lobby button interactable based on the boolean value</summary>
        private void OnLobbyItemSelect(bool value)
        {
            btnJoinLobby.interactable = value;
        }

        // <summary>Called when a room item has been selected or not making the join room button interactabel based on the boolean value</summary>
        private void OnRoomItemSelect(bool value)
        {
            btnJoinRoom.interactable = value;
        }

        /// <summary>Starts a game in offline mode</summary>
        public void StartGameOffline()
        {
            print("start game offline");
        }

        /// <summary>Stops offline mode and returns to main menu buttons</summary>
        public void StopOfflineMode()
        {
            ChangeGroupFocus("MenuButtons");
            NetworkingService.SetOffline(false);
        }

        /// <summary>Stops multiplayer mode and returns to main menu buttons</summary>
        public void StopLobbyList()
        {
            ChangeGroupFocus("MenuButtons");
        }

        // <summary>Stops the room list ui by leaving the lobby</summary>
        public void StopRoomList()
        {
            NetworkingService.LeaveLobby();
        }

        /// <summary>Stops the game setup by leaving the room and returning to room list</summary>
        public void StopGameSetup()
        {
            NetworkingService.LeaveRoom(true);
        }

        /// <summary>Starts the demo game</summary>
        public void StartGame()
        {
            NetworkingService.CloseRoom();
        }

        // <summary>Closes settings ui by changing canvas group focus to menu buttons</summary>
        public void CloseSettings()
        {
            ChangeGroupFocus("MenuButtons");
        }

        // <summary>Called when a room can be created to use the networking service to create a room with given room info</summary>
        public void CreateRoom(string name, int maxPlayers, string password)
        {
            if (!string.IsNullOrEmpty(name))
            {
                NetworkingService.CreateRoom(name, maxPlayers, password);
            }
        }

        // <summary>Called when the join room button is pressed to jon the currently selected room</summary>
        public void JoinSelectedRoom()
        {
            ListItem roomItem = roomListItems.CurrentSelected;
            if (roomItem != null)
            {
                NetworkingService.JoinRoom(roomItem.TxtName.text);
            }
        }

        // <summary>Called when the join lobby button is pressed to jon the currently selected lobby</summary>
        public void JoinSelectedLobby()
        {
            ListItem lobbyItem = lobbyListItems.CurrentSelected;
            if (lobbyItem != null)
            {
                NetworkingService.JoinLobby(lobbyItem.TxtName.text);
            }
        }

        // <summary>Sets the focus to rooms resetting related ui settings in the process</summary>
        private void SetRoomListFocus()
        {
            ChangeGroupFocus("Rooms");
            btnJoinRoom.interactable = false;
        }

        // <summary>Sets the focus to lobbys resetting related ui settings in the process</summary>
        private void SetLobbyListFocus()
        {
            ChangeGroupFocus("Lobbys");
            btnJoinLobby.interactable = false;
        }

        // <summary>Exits the application</summary>
        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}