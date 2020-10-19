using BWolf.Utilities.EventPlanning;
using UnityEngine;

namespace BWolf.Examples.EventPlanning
{
    public class ClockSystem : MonoBehaviour
    {
        [SerializeField]
        private Transform bigPointer = null;

        [SerializeField]
        private Transform smallPointer = null;

        private const float MAX_DEGREES = 360.0f;

        private void Update()
        {
            CalenderDate date = EventCalender.Instance.Date;

            float hourPercentage = date.hour / (float)CalenderDate.CLOCK_HOURS_PER_DAY;
            hourPercentage -= date.hour > CalenderDate.CLOCK_HOURS_PER_DAY ? 1.0f : 0.0f;
            bigPointer.transform.localEulerAngles = new Vector3(0, 0, -(hourPercentage * MAX_DEGREES));

            float minutesPercentage = date.minute / (float)CalenderDate.MINUTES_PER_HOUR;
            smallPointer.transform.localEulerAngles = new Vector3(0, 0, -(minutesPercentage * MAX_DEGREES));
        }
    }
}