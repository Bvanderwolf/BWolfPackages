using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
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
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.AddCallbackListener(SimpleCallbackEvent.JoinedLobby, OnJoinedLobby);
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
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.RemoveCallbackListener(SimpleCallbackEvent.JoinedLobby, OnJoinedLobby);
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
                NetworkingService.ConnectWithDefaultSettings();
            }
            else
            {
                SetLobbyFocus();
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

        // <summary>Called when connected to master, changes to rooms canvas group</summary>
        private void OnConnectedToServer(string message)
        {
            if (!NetworkingService.InOfflineMode)
            {
                SetLobbyFocus();
            }
        }

        // <summary>Called when disconnected, changes to main menu buttons</summary>
        private void OnDisconnected(string cause)
        {
            ChangeGroupFocus("MenuButtons");
        }

        private void OnJoinedLobby(string message)
        {
            SetRoomFocus();
        }

        private void OnLeftLobby(string message)
        {
            SetLobbyFocus();
        }

        private void OnCreatedRoom(string message)
        {
            ChangeGroupFocus("Players");
        }

        private void OnLobbyItemSelect(bool value)
        {
            btnJoinLobby.interactable = value;
        }

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

        public void StopRoomList()
        {
            NetworkingService.LeaveLobby();
        }

        public void CloseSettings()
        {
            ChangeGroupFocus("MenuButtons");
        }

        public void CreateRoom(string name, int maxPlayers, string password)
        {
            if (!string.IsNullOrEmpty(name))
            {
                NetworkingService.CreateRoom(name, maxPlayers, password);
            }
        }

        public void JoinSelectedRoom()
        {
        }

        private void SetRoomFocus()
        {
            ChangeGroupFocus("Rooms");
            btnJoinRoom.interactable = false;
        }

        private void SetLobbyFocus()
        {
            ChangeGroupFocus("Lobbys");
            btnJoinLobby.interactable = false;
        }

        public void JoinSelectedLobby()
        {
            ListItem lobbyItem = lobbyListItems.CurrentSelected;
            if (lobbyItem != null)
            {
                int count;
                if (int.TryParse(lobbyItem.PlayerCount, out count) && count >= 0)
                {
                    NetworkingService.JoinLobby(lobbyItem.TxtName.text);
                }
            }
        }

        public void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}