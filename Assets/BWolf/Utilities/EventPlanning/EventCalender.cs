using BWolf.Behaviours.SingletonBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.EventPlanning
{
    public class EventCalender : SingletonBehaviour<EventCalender>
    {
        [SerializeField]
        private float timeScale = 1f;

        private List<DatedEvent> events = new List<DatedEvent>();

        private EventDate date = new EventDate();

        public EventDate Date
        {
            get { return date; }
        }

        protected override void Awake()
        {
            base.Awake();

            if (!isDuplicate)
            {
                date.AddSeconds(Time.realtimeSinceStartup);
            }
        }

        private void Update()
        {
            date.AddSeconds(Time.unscaledDeltaTime * timeScale);

            CheckEvents();
        }

        public void AddEvent(DatedEvent datedEvent)
        {
            if (datedEvent.Date > date)
            {
                events.Add(datedEvent);
            }
            else
            {
                Debug.LogWarning("Tried adding dated event with lower date than current date :: make sure the date is greater than current date");
            }
        }

        private void CheckEvents()
        {
            for (int i = events.Count - 1; i >= 0; i--)
            {
                if (events[i].Date.Equals(Date))
                {
                    events[i].Execute();
                    events.RemoveAt(i);
                }
            }
        }
    }

    public struct EventDate
    {
        private double _seconds;

        public int minute { get; set; }
        public int hour { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }

        private const int SECONDS_PER_MINUTE = 60;
        private const int MINUTES_PER_HOUR = 60;
        private const int HOURS_PER_DAY = 24;
        private const int DAYS_PER_MONTH = 30;
        private const int MONTHS_PER_YEAR = 12;

        public int second
        {
            get { return (int)_seconds; }
            set { _seconds = value; }
        }

        public void AddSeconds(double seconds)
        {
            _seconds += seconds;

            if (_seconds > SECONDS_PER_MINUTE)
            {
                minute++;
                _seconds -= SECONDS_PER_MINUTE;
            }

            UpdateMinutes();
        }

        private void UpdateMinutes()
        {
            if (minute == MINUTES_PER_HOUR)
            {
                hour++;
                minute = 0;
            }

            UpdateHours();
        }

        private void UpdateHours()
        {
            if (hour == HOURS_PER_DAY)
            {
                day++;
                hour = 0;
            }

            UpdateDays();
        }

        private void UpdateDays()
        {
            if (day > DAYS_PER_MONTH)
            {
                month++;
                day = 0;
            }

            UpdateMonths();
        }

        private void UpdateMonths()
        {
            if (month > MONTHS_PER_YEAR)
            {
                year++;
                month = 0;
            }
        }

        public bool Equals(EventDate dt2)
        {
            return year == dt2.year
               && month == dt2.month
               && day == dt2.day
               && hour == dt2.hour
               && minute == dt2.minute
               && second == dt2.second;
        }

        public override string ToString()
        {
            return $"{hour}:{minute}:{second} {day}/{month}/{year}";
        }

        public static bool operator >(EventDate dt1, EventDate dt2)
        {
            return dt1.year > dt2.year
                || dt1.month > dt2.month
                || dt1.day > dt2.day
                || dt1.hour > dt2.hour
                || dt1.minute > dt2.minute
                || dt1.second > dt2.second;
        }

        public static bool operator <(EventDate dt1, EventDate dt2)
        {
            return !(dt1 > dt2);
        }
    }
}