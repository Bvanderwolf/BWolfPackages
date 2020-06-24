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

        [SerializeField]
        private JoinRoomForm joinRoomForm = null;

        [Header("Settings")]
        [SerializeField]
        private List<RoomListItem> listItems = null;

        [Header("Events")]
        [SerializeField]
        private OnCreateRoomFinished onCreateRoomFinished = null;

        [SerializeField]
        private OnJoinProtocolFinished JoinProtocolFinished = null;

        private List<RoomData> dataShowing = new List<RoomData>();

        public const int DemoGameMaxPlayers = 2;
        private const int demoMaxRoomsInLobby = 5;

        private void Start()
        {
            ToggleCreateRoomForm();
            ToggleJoinRoomForm();

            btnCreate.onClick.AddListener(ToggleCreateRoomForm);
            btnJoin.onClick.AddListener(OnJoinbuttonClick);

            joinRoomForm.AddListener(OnJoinRoomFinish);

            NetworkingService.AddRoomListListener(UpdateListItemsWithRoomData);
        }

        private void OnDestroy()
        {
            btnCreate.onClick.RemoveListener(ToggleCreateRoomForm);
            btnJoin.onClick.RemoveListener(OnJoinbuttonClick);

            joinRoomForm.RemoveListener(OnJoinRoomFinish);

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

        /// <summary>Called when the join butotn has been click, it either starts password form or fires joinprotocol finished event based on whether the room has a password  or not</summary>
        private void OnJoinbuttonClick()
        {
            RoomListItem selectedItem = ((RoomListItem)CurrentSelected);
            if (selectedItem.HasKey)
            {
                ToggleJoinRoomForm();
                btnJoin.interactable = false;
                SetupJoinRoomFormWithItem(selectedItem);
            }
            else
            {
                JoinProtocolFinished.Invoke(selectedItem.TxtName.text);
            }
        }

        /// <summary>Toggles the active state of the create room form</summary>
        public void ToggleCreateRoomForm()
        {
            GameObject form = createRoomForm.gameObject;
            form.SetActive(!form.activeInHierarchy);
        }

        /// <summary>Toggles the active state of the join room form</summary>
        public void ToggleJoinRoomForm()
        {
            GameObject form = joinRoomForm.gameObject;
            form.SetActive(!form.activeInHierarchy);
        }

        /// <summary>called when the finish button has been pressed on the create room form to start creating the room with given information</summary>
        public void CreateRoomFinish(string nameEntered, string passwordEntered)
        {
            ToggleCreateRoomForm();
            onCreateRoomFinished.Invoke(nameEntered, DemoGameMaxPlayers, passwordEntered);
        }

        public void OnJoinRoomFinish(bool value, string nameOfRoom)
        {
            ToggleJoinRoomForm();
            if (value)
            {
                JoinProtocolFinished.Invoke(nameOfRoom);
            }
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
            foreach (RoomData data in newData) { print(data); }
            for (int i = 0; i < newData.Count; i++)
            {
                bool wasListed = false;
                for (int j = dataShowing.Count - 1; j >= 0; j--)
                {
                    //if the room is already in data showing...
                    if (newData[i].Name == dataShowing[j].Name)
                    {
                        if (newData[i].RemovedFromList || !newData[i].IsOpen)
                        {
                            //remove deleted and closed rooms from data showing
                            dataShowing.RemoveAt(j);
                        }
                        else
                        {
                            //update data showing with open and not deleted rooms
                            dataShowing[j] = newData[i];
                        }

                        //set was listed to true
                        wasListed = true;
                        break;
                    }
                }

                //if a room wasn't listed in data showing and it isn't removed and is open, add it to data showing
                if (!wasListed && !newData[i].RemovedFromList && newData[i].IsOpen)
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

        private void SetupJoinRoomFormWithItem(RoomListItem item)
        {
            for (int i = 0; i < dataShowing.Count; i++)
            {
                if (item.TxtName.text == dataShowing[i].Name)
                {
                    joinRoomForm.AddRequirement(dataShowing[i].Key);
                    joinRoomForm.AddNameOfRoom(dataShowing[i].Name);
                    break;
                }
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

        [System.Serializable]
        public class OnJoinProtocolFinished : UnityEvent<string> { }
    }
}