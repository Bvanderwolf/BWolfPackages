using BWolf.Wrappers.PhotonSDK;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class MainUIHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CanvasGroup[] canvasGroups = null;

        [SerializeField]
        private Button multiplayerUIButton = null;

        [SerializeField]
        private Button offlineButton = null;

        private Dictionary<string, CanvasGroup> canvasGroupDictionary = new Dictionary<string, CanvasGroup>();

        private void Awake()
        {
            //add listeners
            multiplayerUIButton.onClick.AddListener(OnMultiplayerButtonClick);
            offlineButton.onClick.AddListener(OnOfflineButtonClick);

            //create dictionary entries using group name as key
            foreach (CanvasGroup group in canvasGroups)
            {
                canvasGroupDictionary.Add(group.name, group);
            }

            //add listener for connected to master and disconnect callback
            NetworkingService.AddCallbackListener(CallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.AddCallbackListener(CallbackEvent.Disconnected, OnDisconnected);

            //change group focus to menu buttons
            ChangeGroupFocus("MenuButtons");
        }

        private void OnDestroy()
        {
            multiplayerUIButton.onClick.RemoveListener(OnMultiplayerButtonClick);
            offlineButton.onClick.RemoveListener(OnOfflineButtonClick);

            NetworkingService.RemoveCallbackListener(CallbackEvent.ConnectedToMaster, OnConnectedToServer);
            NetworkingService.RemoveCallbackListener(CallbackEvent.Disconnected, OnDisconnected);
        }

        /// <summary>Uses canvas groups to only show the given group name in the ui</summary>
        private void ChangeGroupFocus(string newFocusedGroupName)
        {
            if (canvasGroupDictionary.ContainsKey(newFocusedGroupName))
            {
                foreach (var groupEntry in canvasGroupDictionary)
                {
                    groupEntry.Value.alpha = groupEntry.Key == newFocusedGroupName ? 1 : 0;
                }
            }
        }

        /// <summary>Starts connection using the default settings</summary>
        private void OnMultiplayerButtonClick()
        {
            NetworkingService.ConnectWithDefaultSettings();
        }

        /// <summary>Starts offline mode and switches to offline mode buttons</summary>
        private void OnOfflineButtonClick()
        {
            ChangeGroupFocus("OfflineButtons");
            NetworkingService.SetOffline(true);
        }

        // <summary>Called when connected to master, changes to rooms canvas group</summary>
        private void OnConnectedToServer(string message)
        {
            ChangeGroupFocus("Rooms");
        }

        // <summary>Called when disconnected, changes to main menu buttons</summary>
        private void OnDisconnected(string cause)
        {
            ChangeGroupFocus("MenuButtons");
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
        public void StopMultiplayerMode()
        {
            ChangeGroupFocus("MenuButtons");
        }
    }
}