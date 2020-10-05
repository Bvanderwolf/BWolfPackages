using BWolf.Utilities.EventPlanning;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.EventPlanning
{
    public class EventPlanManager : MonoBehaviour
    {
        private EventPlannerPart[] parts;

        private void Awake()
        {
            parts = GetComponentsInChildren<EventPlannerPart>();
        }

        public void OnAddEventClick()
        {
            Dictionary<string, int> partPairs = new Dictionary<string, int>();
            foreach (EventPlannerPart part in parts)
            {
                partPairs.Add(part.NameOfPart, part.Number);
            }

            EventCalender.Instance.AddEvent(new DatedEvent(new EventDate
            {
                second = partPairs["second"],
                minute = partPairs["minute"],
                hour = partPairs["hour"],
                day = partPairs["day"],
                month = partPairs["month"],
                year = partPairs["year"],
            },
            OnEvent));
        }

        private void OnEvent(DatedEvent datedEvent)
        {
            print("Event went of at " + datedEvent.Date.ToString());
        }
    }
}