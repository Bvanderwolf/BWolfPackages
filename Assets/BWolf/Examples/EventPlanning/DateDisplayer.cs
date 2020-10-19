using BWolf.Utilities.EventPlanning;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.EventPlanning
{
    public class DateDisplayer : MonoBehaviour
    {
        [SerializeField]
        private Text seconds = null;

        [SerializeField]
        private Text minutes = null;

        [SerializeField]
        private Text hours = null;

        [SerializeField]
        private Text days = null;

        [SerializeField]
        private Text months = null;

        [SerializeField]
        private Text years = null;

        private void Update()
        {
            CalenderDate date = EventCalender.Instance.Date;
            seconds.text = "Seconds: " + date.second;
            minutes.text = "Minutes " + date.minute;
            hours.text = "Hours: " + date.hour;
            days.text = "Days: " + date.day;
            months.text = "Months: " + date.month;
            years.text = "Years: " + date.year;
        }
    }
}