using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BWolf.Examples.PhotonWrapper
{
    public class ListItemsUI : MonoBehaviour
    {
        [SerializeField]
        private List<ListItem> listItems = null;

        public ListItem CurrentSelected { get; private set; } = ListItem.Empty;

        private void Awake()
        {
            foreach (ListItem item in listItems)
            {
                var entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick, callback = new EventTrigger.TriggerEvent() };
                entry.callback.AddListener(OnClickedItem);
                item.EventTriggers.triggers.Add(entry);
            }
        }

        public void ResetCurrentSelected()
        {
            CurrentSelected = ListItem.Empty;
        }

        private void OnClickedItem(BaseEventData data)
        {
            foreach (ListItem item in listItems)
            {
                if (item.EventTriggers.gameObject == data.selectedObject)
                {
                    CurrentSelected = item;
                    break;
                }
            }
        }

        [System.Serializable]
        public struct ListItem
        {
#pragma warning disable 0649
            public Text TxtName;
            public Text TxtCount;
            public EventTrigger EventTriggers;
#pragma warning restore 0649

            public string PlayerCount
            {
                get
                {
                    string s = TxtCount.text;
                    return s.Substring(0, s.IndexOf('/'));
                }
            }

            public static ListItem Empty
            {
                get
                {
                    ListItem item;
                    item.TxtName = null;
                    item.TxtCount = null;
                    item.EventTriggers = null;
                    return item;
                }
            }

            public static bool IsEmpty(ListItem item)
            {
                return item.TxtName == null && item.TxtCount == null && item.EventTriggers == null;
            }

            public override string ToString()
            {
                return string.Format("name: {0}, count: {1}", TxtName.text, TxtCount.text);
            }
        }
    }
}