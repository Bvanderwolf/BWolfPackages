using BWolf.Utilities.ProcessQueues;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(fileName = "MonologueRequestChannel", menuName = "SO Event Channels/Monologue")]
    public class MonologueEventChannel : ProcessEventChannel<Monologue>
    {
    }
}