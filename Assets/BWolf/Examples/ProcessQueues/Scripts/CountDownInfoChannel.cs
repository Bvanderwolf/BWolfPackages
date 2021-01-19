using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Examples.ProcessQueues
{
    [CreateAssetMenu(fileName = "CountDownInfoChannel", menuName = "SO Event Channels/CountDownInfo")]
    public class CountDownInfoChannel : ProcessEventChannel<CountDownInfo>
    {
    }
}