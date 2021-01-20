using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(fileName = "DialogueProcessQueue", menuName = "Queues/Dialogue")]
    public class DialogueProcessQueue : ProcessQueue<Dialogue>
    {
    }
}