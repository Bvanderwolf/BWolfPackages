using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(fileName = "DialogueRequestChannel", menuName = "SO Event Channels/Dialogue")]
    public class DialogueEventChannel : ProcessEventChannel<Dialogue>
    {
    }
}