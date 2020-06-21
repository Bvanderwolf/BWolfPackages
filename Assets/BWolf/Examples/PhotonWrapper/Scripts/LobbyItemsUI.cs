using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class LobbyItemsUI : ListItemsUI
    {
        [SerializeField]
        private List<LobbyListItem> listItems = null;

        private List<LobbyData> lobbyInfo = new List<LobbyData>();

        private void Start()
        {
            NetworkingService.AddLobbyStatisticsListener(UpdateItemsWithLobbyData);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveLobbyStatisticsListener(UpdateItemsWithLobbyData);
        }

        /// <summary>Called on awake to setup lobby list item trigger events</summary>
        protected override void SetupListItemTriggers()
        {
            foreach (LobbyListItem item in listItems)
            {
                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() };
                entry.callback.AddListener(OnClickedItem);
                item.EventTriggers.triggers.Add(entry);
            }
        }

        /// <summary>Called when an item has been clicked to set the current selected reference and invoke the OnSelect event</summary>
        protected override void OnClickedItem(BaseEventData data)
        {
            foreach (ListItem item in listItems)
            {
                if (item.EventTriggers.gameObject == data.selectedObject)
                {
                    CurrentSelected = item;
                    OnSelect(true);
                    break;
                }
            }
        }

        /// <summary>Updates lobby list items with given lobby data </summary>
        private void UpdateItemsWithLobbyData(List<LobbyData> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                listItems[i].SetLobbyName(data[i].Name);
                listItems[i].SetPlayerCount(data[i].PlayerCount, NetworkingService.MaxPlayersOnServer);
                listItems[i].SetRoomCount(data[i].RoomCount, NetworkingService.MaxPlayersOnServer);
            }
        }

        /// <summary>Lobby is a list item with some additional properties and functionalities</summary>
        [System.Serializable]
        public class LobbyListItem : ListItem
        {
            public Text TxtRoomCount;

            public void SetLobbyName(string name)
            {
                TxtName.text = string.IsNullOrEmpty(name) ? "Default" : name;
            }

            public void SetRoomCount(int roomCount, int maxPlayers)
            {
                TxtRoomCount.text = string.Format("{0}/10", roomCount);
            }
        }
    }
}