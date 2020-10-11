using BWolf.Utilities.CharacterDialogue;
using UnityEngine;

namespace BWolf.Examples.CharacterDialogue
{
    public class DialogueStarter : MonoBehaviour
    {
        [SerializeField]
        private Dialogue demoDialogue = null;

        [ContextMenu("Demo")]
        public void Demo()
        {
            DialogueSystem.Instance.StartDialogue(demoDialogue);
        }
    }
}