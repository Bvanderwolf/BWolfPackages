using BWolf.Behaviours.SingletonBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.EventPlanning
{
    public class EventCalender : SingletonBehaviour<EventCalender>
    {
        [SerializeField]
        private float timeScale = 1f;

        [SerializeField]
        private int startingHour = 12;

        private List<DatedEvent> events = new List<DatedEvent>();

        private CalenderDate date;

        public CalenderDate Date
        {
            get { return date; }
        }

        protected override void Awake()
        {
            base.Awake();

            if (!isDuplicate)
            {
                date = new CalenderDate(startingHour);
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

    public struct CalenderDate
    {
        private double _seconds;

        public int minute { get; set; }
        public int hour { get; set; }
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public bool night { get; private set; }

        public const int SECONDS_PER_MINUTE = 60;
        public const int MINUTES_PER_HOUR = 60;
        public const int HOURS_PER_DAY = 24;
        public const int CLOCK_HOURS_PER_DAY = 12;
        public const int DAYS_PER_MONTH = 30;
        public const int MONTHS_PER_YEAR = 12;

        private const int NIGHT_START_HOUR = 18;
        private const int NIGHT_END_HOUR = 6;

        public CalenderDate(int startingHour)
        {
            _seconds = 0.0d;
            minute = 0;
            hour = Mathf.Clamp(startingHour, 0, HOURS_PER_DAY);
            night = hour > NIGHT_START_HOUR || hour < NIGHT_END_HOUR;
            day = 0;
            month = 0;
            year = 0;
        }

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

        public bool Equals(CalenderDate dt2)
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

        public static bool operator >(CalenderDate dt1, CalenderDate dt2)
        {
            return dt1.year > dt2.year
                || dt1.month > dt2.month
                || dt1.day > dt2.day
                || dt1.hour > dt2.hour
                || dt1.minute > dt2.minute
                || dt1.second > dt2.second;
        }

        public static bool operator <(CalenderDate dt1, CalenderDate dt2)
        {
            return !(dt1 > dt2);
        }
    }
}