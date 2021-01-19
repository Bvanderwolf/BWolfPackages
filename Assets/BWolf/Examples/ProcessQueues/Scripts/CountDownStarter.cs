using UnityEngine;

namespace BWolf.Examples.ProcessQueues
{
    public class CountDownStarter : MonoBehaviour
    {
        [Header("Count down info")]
        [SerializeField]
        private CountDownInfo[] info = null;

        private void Awake()
        {
            CountDownSystem system = GetComponent<CountDownSystem>();
            foreach (CountDownInfo i in info)
            {
                system.StartProcess(i);
            }
        }
    }
}