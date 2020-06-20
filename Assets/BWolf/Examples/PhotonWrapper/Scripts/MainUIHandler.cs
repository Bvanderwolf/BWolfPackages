using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    using ListItem = ListItemsUI.ListItem;

    public class MainUIHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Text txtConnectionState = null;

        [SerializeField]
        private CanvasGroup[] canvasGroups = null;

        [SerializeField]
        private Button btnMultiplayer = null;

        [SerializeField]
        private Button btnOffline = null;

        [SerializeField]
        private ListItemsUI lobbyListItems = null;

        private Dictionary<string, CanvasGroup> canvasGroupDictionary = new Dictionary<string, CanvasGroup>();

        private void Awake()
        {
            //add listeners
            btnMultiplayer.onClick.AddListener(OnMultiplayerButtonClick);
            btnOffline.onClick.AddListener(OnOfflineButtonClick);

            //create dictionary entries using group name as key
            foreach (CanvasGroup group in canvasGroups)
            {
                canvasGroupDictionary.Add(group.name, group);
            }

            //add listeners for callbacks
            NetworkingService.AddCallbackListener(CallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.AddCallbackListener(CallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.AddCallbackListener(CallbackEvent.JoinedLobby, OnJoinedLobby);
            NetworkingService.AddCallbackListener(CallbackEvent.LeftLobby, OnLeftLobby);

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

            //remove listeners on destroy
            NetworkingService.RemoveCallbackListener(CallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.RemoveCallbackListener(CallbackEvent.Disconnected, OnDisconnected);
            NetworkingService.RemoveCallbackListener(CallbackEvent.JoinedLobby, OnJoinedLobby);
            NetworkingService.RemoveCallbackListener(CallbackEvent.LeftLobby, OnLeftLobby);
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
                ChangeGroupFocus("Lobbys");
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

        // <summary>Called when connected to master, changes to rooms canvas group</summary>
        private void OnConnectedToServer(string message)
        {
            if (!NetworkingService.InOfflineMode)
            {
                ChangeGroupFocus("Lobbys");
            }
        }

        // <summary>Called when disconnected, changes to main menu buttons</summary>
        private void OnDisconnected(string cause)
        {
            ChangeGroupFocus("MenuButtons");
        }

        private void OnJoinedLobby(string message)
        {
            ChangeGroupFocus("Rooms");
        }

        private void OnLeftLobby(string message)
        {
            ChangeGroupFocus("Lobbys");
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

        public void CreateRoom()
        {
        }

        public void JoinSelectedRoom()
        {
        }

        public void JoinSelectedLobby()
        {
            ListItem lobbyItem = lobbyListItems.CurrentSelected;
            if (!ListItem.IsEmpty(lobbyItem))
            {
                int count;
                if (int.TryParse(lobbyItem.PlayerCount, out count) && count >= 0)
                {
                    NetworkingService.JoinLobby(lobbyItem.TxtName.text);
                }
            }
        }
    }
}