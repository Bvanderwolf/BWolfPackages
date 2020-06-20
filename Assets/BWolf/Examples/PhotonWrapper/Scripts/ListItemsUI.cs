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

        public void OnSelect(bool value)
        {
            onSelect?.Invoke(value);
        }

        public void AddListener(Action<bool> onSelect)
        {
            this.onSelect += onSelect;
        }

        public void RemoveListener(Action<bool> onSelect)
        {
            this.onSelect -= onSelect;
        }

        public void ResetCurrentSelected()
        {
            CurrentSelected = null;
        }

        public class ListItem
        {
            public Text TxtName = null;
            public Text TxtPlayerCount = null;
            public EventTrigger EventTriggers = null;

            public string PlayerCount
            {
                get
                {
                    string s = TxtPlayerCount.text;
                    return s.Substring(0, s.IndexOf('/'));
                }
            }

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