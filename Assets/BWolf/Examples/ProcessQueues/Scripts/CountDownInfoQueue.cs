using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Examples.ProcessQueues
{
    [CreateAssetMenu(fileName = "CountDownInfoQueue", menuName = "Queues/CountDownInfo")]
    public class CountDownInfoQueue : ProcessQueue<CountDownInfo>
    {
    }
}