using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(fileName = "MonologueProcessQueue", menuName = "Queues/Monologue")]
    public class MonologueProcessQueue : ProcessQueue<Monologue>
    {
    }
}