using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.EventPlanning
{
    public class EventPlannerPart : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string nameOfDatePart = string.Empty;

        [SerializeField]
        private int maxNumber = 0;

        [Header("References")]
        [SerializeField]
        private Text txtHeader = null;

        [SerializeField]
        private Text txtNumber = null;

        public string NameOfPart
        {
            get { return nameOfDatePart; }
        }

        public int Number { get; private set; }

        private void Awake()
        {
            txtHeader.text = nameOfDatePart;
            SetNumberToText();
        }

        public void OnUpClick()
        {
            Number++;
            if (Number > maxNumber)
            {
                Number = maxNumber;
            }

            SetNumberToText();
        }

        public void OnDownClick()
        {
            Number--;
            if (Number < 0)
            {
                Number = 0;
            }

            SetNumberToText();
        }

        private void SetNumberToText()
        {
            txtNumber.text = Number.ToString();
        }
    }
}