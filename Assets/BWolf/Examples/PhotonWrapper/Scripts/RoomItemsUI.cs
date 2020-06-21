using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class RoomItemsUI : ListItemsUI
    {
        [SerializeField]
        private List<RoomListItem> listItems = null;

        private void Start()
        {
            NetworkingService.AddRoomListListener(UpdateListItemsWithRoomData);
        }

        private void OnDestroy()
        {
            NetworkingService.RemoveRoomListListener(UpdateListItemsWithRoomData);
        }

        protected override void SetupListItemTriggers()
        {
            foreach (RoomListItem item in listItems)
            {
                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() };
                entry.callback.AddListener(OnClickedItem);
                item.EventTriggers.triggers.Add(entry);
            }
        }

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

        private void UpdateListItemsWithRoomData(List<RoomData> data)
        {
            foreach (RoomData item in data)
            {
                print(item);
            }
        }

        /// <summary>Lobby is a list item with some additional properties and functionalities</summary>
        [System.Serializable]
        public class RoomListItem : ListItem
        {
            public Image LockImage;

            public bool IsLocked
            {
                get { return LockImage.enabled; }
            }
        }
    }
}