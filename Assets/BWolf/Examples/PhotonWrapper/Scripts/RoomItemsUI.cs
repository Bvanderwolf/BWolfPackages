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
        private Button btnCreate = null;

        [SerializeField]
        private CreateRoomForm createRoomForm = null;

        [Header("Settings")]
        [SerializeField]
        private List<RoomListItem> listItems = null;

        [Header("Events")]
        [SerializeField]
        private OnCreateRoomFinished onCreateRoomFinished = null;

        private List<RoomData> dataShowing = new List<RoomData>();

        public const int DemoGameMaxPlayers = 2;
        private const int demoMaxRoomsInLobby = 5;

        private void Start()
        {
            ToggleCreateRoomForm();
            btnCreate.onClick.AddListener(ToggleCreateRoomForm);
            NetworkingService.AddRoomListListener(UpdateListItemsWithRoomData);
        }

        private void OnDestroy()
        {
            btnCreate.onClick.RemoveListener(ToggleCreateRoomForm);
            NetworkingService.RemoveRoomListListener(UpdateListItemsWithRoomData);
        }

        /// <summary>Sets up the room list event tirggers</summary>
        protected override void SetupListItemTriggers()
        {
            foreach (RoomListItem item in listItems)
            {
                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() };
                entry.callback.AddListener(OnClickedItem);
                item.EventTriggers.triggers.Add(entry);
            }
        }

        /// <summary>Called when a room item has been clicked to set current selected and fire the on select event</summary>
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

        /// <summary>Toggles the active state of the create room form</summary>
        public void ToggleCreateRoomForm()
        {
            GameObject form = createRoomForm.gameObject;
            form.SetActive(!form.activeInHierarchy);
        }

        /// <summary>called when the finish button has been pressed on the create room form to start creating the room with given information</summary>
        public void CreateRoomFinish(string nameEntered, string passwordEntered)
        {
            ToggleCreateRoomForm();
            onCreateRoomFinished.Invoke(nameEntered, DemoGameMaxPlayers, passwordEntered);
        }

        /// <summary>Returns whether given roomname is already a listed room's name</summary>
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

        /// <summary>Updates the lits items based on given room data</summary>
        private void UpdateListItemsWithRoomData(List<RoomData> data)
        {
            ClearListItemUI();
            UpdateDataShowing(data);
            CreateItemsBasedOnShowingData();
            SetupListItemTriggers();
            CheckCreateRoomAbility();
        }

        /// <summary>destroys all room item ui objects except for head</summary>
        private void ClearListItemUI()
        {
            //clear list items
            listItems.Clear();

            //reset current selected if an item was selected
            if (CurrentSelected != null)
            {
                CurrentSelected = null;
                OnSelect(false);
            }

            //destroy all list item game objects
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
                            dataShowing[j] = newData[i];
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
                listItem.SetPlayerCount(item.PlayerCount, DemoGameMaxPlayers);
                listItem.EventTriggers.GetComponent<Selectable>().interactable = !item.IsFull;
                listItem.KeyImage.enabled = item.HasKey;
                listItems.Add(listItem);
            }
        }

        /// <summary>Sets the interactability of the create button based on ammount of room data showing right now</summary>
        private void CheckCreateRoomAbility()
        {
            btnCreate.interactable = dataShowing.Count < demoMaxRoomsInLobby;
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