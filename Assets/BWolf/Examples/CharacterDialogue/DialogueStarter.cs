using BWolf.Utilities.CharacterDialogue;
using UnityEngine;

namespace BWolf.Examples.CharacterDialogue
{
    public class DialogueStarter : MonoBehaviour
    {
        [SerializeField]
        private DialogueChoice choice = DialogueChoice.DemoDialogue1;

        [SerializeField]
        private Dialogue demoDialogue1 = null;

        [SerializeField]
        private Dialogue demoDialogue2 = null;

        [SerializeField]
        private Monologue demoMonologue = null;

        [Header("Channels broadcasting on")]
        [SerializeField]
        private DialogueEventChannel dialogueChannel = null;

        [SerializeField]
        private MonologueEventChannel monologueChannel = null;

        public void StartDialogue()
        {
            switch (choice)
            {
                case DialogueChoice.DemoDialogue1:
                    dialogueChannel.RaiseEvent(demoDialogue1);
                    break;

                case DialogueChoice.DemoDialogue2:
                    dialogueChannel.RaiseEvent(demoDialogue2);
                    break;

                case DialogueChoice.DemoMonologue:
                    monologueChannel.RaiseEvent(demoMonologue);
                    break;
            }
        }

        private enum DialogueChoice
        {
            DemoDialogue1,
            DemoDialogue2,
            DemoMonologue
        }
    }
}