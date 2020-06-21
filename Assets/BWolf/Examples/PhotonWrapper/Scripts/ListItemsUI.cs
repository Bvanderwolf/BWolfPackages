using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class ListItemsUI : MonoBehaviour
    {
        [SerializeField]
        private Button joinButton = null;

        private event Action<bool> onSelect;

        public ListItem CurrentSelected { get; protected set; }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && CurrentSelected != null)
            {
                GameObject current = EventSystem.current.currentSelectedGameObject;
                if (current != CurrentSelected.EventTriggers.gameObject && current != joinButton.gameObject)
                {
                    CurrentSelected = null;
                    OnSelect(false);
                }
            }
        }

        /// <summary>Fires the OnSelect event with given value as argument</summary>
        public void OnSelect(bool value)
        {
            onSelect?.Invoke(value);
        }

        /// <summary>Adds a listener to the OnSelect event</summary>
        public void AddListener(Action<bool> onSelect)
        {
            this.onSelect += onSelect;
        }

        /// <summary>removes listener from the ONSelect event</summary>
        public void RemoveListener(Action<bool> onSelect)
        {
            this.onSelect -= onSelect;
        }

        /// <summary>Resets current selected value</summary>
        public void ResetCurrentSelected()
        {
            CurrentSelected = null;
        }

        /// <summary>List item is used to reference ui items containing a name a playercount and a trigger event to see if they are selected or not</summary>
        public class ListItem
        {
            public Text TxtName = null;
            public Text TxtPlayerCount = null;
            public EventTrigger EventTriggers = null;

            /// <summary>Returns the playercount number as string</summary>
            public string PlayerCount
            {
                get
                {
                    string s = TxtPlayerCount.text;
                    return s.Substring(0, s.IndexOf('/'));
                }
            }

            /// <summary>Sets the player count text for this list item</summary>
            public void SetPlayerCount(int playerCount)
            {
                TxtPlayerCount.text = string.Format("{0}/20", playerCount);
            }

            public override string ToString()
            {
                return string.Format("name: {0}, count: {1}", TxtName.text, TxtPlayerCount.text);
            }
        }
    }
}