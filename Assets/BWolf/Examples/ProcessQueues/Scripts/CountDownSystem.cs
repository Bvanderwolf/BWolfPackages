using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BWolf.Utilities.ProcessQueues;

namespace BWolf.Examples.ProcessQueues
{
    public class CountDownSystem : BroadcastingProcessManager<CountDownInfo>
    {
        [Header("Scene References")]
        [SerializeField]
        private Text txtCountDown = null;

        [Header("Project References")]
        [SerializeField]
        private CountDownInfoQueue queue = null;

        [Header("Channel Broadcasting on")]
        [SerializeField]
        private CountDownInfoChannel channel = null;

        protected override ProcessQueue<CountDownInfo> ProcessQueue
        {
            get { return queue; }
        }

        protected override ProcessEventChannel<CountDownInfo> EventChannel
        {
            get { return channel; }
        }

        private void UpdateTextWithNumber(int number)
        {
            txtCountDown.text = number.ToString();
        }

        protected override void OnProcessStart(CountDownInfo info)
        {
            base.OnProcessStart(info);

            Debug.Log($"started counting down with {info} info");
        }

        protected override void OnCheckedForDeque(CountDownInfo oldInfo, CountDownInfo nextInfo, bool dequeued)
        {
            base.OnCheckedForDeque(oldInfo, nextInfo, dequeued);

            if (dequeued)
            {
                Debug.Log($"started counting down with {nextInfo} info after dequeue of {oldInfo}");
            }
            else
            {
                Debug.Log($"stopped counting down");
                txtCountDown.text = string.Empty;
            }
        }

        protected override IEnumerator DoProcess(CountDownInfo info)
        {
            int number = info.start;
            while (number != 0)
            {
                number--;

                UpdateTextWithNumber(number);
                yield return new WaitForSeconds(info.interval);
            }
        }
    }
}