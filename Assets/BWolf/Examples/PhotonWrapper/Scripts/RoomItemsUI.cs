using BWolf.Wrappers.PhotonSDK;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class RoomItemsUI : ListItemsUI
    {
        [SerializeField]
        private GameObject prefabRoomListItem = null;

        [SerializeField]
        private CreateRoomForm createRoomForm = null;

        [Header("Settings")]
        [SerializeField]
        private List<RoomListItem> listItems = null;

        [Header("Events")]
        [SerializeField]
        private OnCreateRoomFinished onCreateRoomFinished = null;

        private List<RoomData> dataShowing = new List<RoomData>();

        private const int demoGameMaxPlayers = 2;

        private void Start()
        {
            ToggleCreateRoomForm();
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
                    print(CurrentSelected.EventTriggers.gameObject);
                    OnSelect(true);
                    break;
                }
            }
        }

        public void ToggleCreateRoomForm()
        {
            GameObject form = createRoomForm.gameObject;
            form.SetActive(!form.activeInHierarchy);
        }

        public void CreateRoomFinish(string nameEntered, string passwordEntered)
        {
            ToggleCreateRoomForm();
            onCreateRoomFinished.Invoke(nameEntered, demoGameMaxPlayers, passwordEntered);
        }

        public bool IsListedRoom(string roomName)
        {
            if (dataShowing.Count == 0) { return false; }

            for (int i = 0; i < dataShowing.Count; i++)
            {
                if (dataShowing[i].Name == roomName)
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateListItemsWithRoomData(List<RoomData> data)
        {
            ClearListItemGameObjects();
            UpdateDataShowing(data);
            CreateItemsBasedOnShowingData();
            SetupListItemTriggers();
        }

        /// <summary>destroys all room item ui objects except for head</summary>
        private void ClearListItemGameObjects()
        {
            listItems.Clear();

            for (int i = transform.childCount - 1; i >= 1; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        /// <summary>updates data showing using given new data</summary>
        private void UpdateDataShowing(List<RoomData> newData)
        {
            for (int i = 0; i < newData.Count; i++)
            {
                bool wasListed = false;
                for (int j = dataShowing.Count - 1; j >= 0; j--)
                {
                    if (newData[i].Name == dataShowing[j].Name)
                    {
                        if (newData[i].RemovedFromList)
                        {
                            dataShowing.RemoveAt(j);
                        }
                        else
                        {
                            dataShowing[j].Override(newData[i]);
                        }

                        wasListed = true;
                        break;
                    }
                }

                if (!wasListed && !newData[i].RemovedFromList)
                {
                    dataShowing.Add(newData[i]);
                }
            }
        }

        /// <summary>Creates ui elements based on the data showing list</summary>
        private void CreateItemsBasedOnShowingData()
        {
            foreach (RoomData item in dataShowing)
            {
                RoomListItem listItem = new RoomListItem();
                ListItemMembers members = Instantiate(prefabRoomListItem, transform).GetComponent<ListItemMembers>();
                listItem.TxtName = members.MemberDictionary["RoomName"].GetComponent<Text>();
                listItem.TxtPlayerCount = members.MemberDictionary["PlayerCount"].GetComponent<Text>();
                listItem.EventTriggers = members.MemberDictionary["Interactable"].GetComponent<EventTrigger>();
                listItem.KeyImage = members.MemberDictionary["KeyImage"].GetComponent<Image>();

                listItem.TxtName.text = item.Name;
                listItem.SetPlayerCount(item.PlayerCount, demoGameMaxPlayers);
                listItem.KeyImage.enabled = item.HasKey;
                listItems.Add(listItem);
            }
        }

        /// <summary>Lobby is a list item with some additional properties and functionalities</summary>
        [System.Serializable]
        public class RoomListItem : ListItem
        {
            public Image KeyImage;

            public bool HasKey
            {
                get { return KeyImage.enabled; }
            }
        }

        [System.Serializable]
        public class OnCreateRoomFinished : UnityEvent<string, int, string> { }
    }
}