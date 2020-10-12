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

        private void Awake()
        {
            StartDialogue();
        }

        public void StartDialogue()
        {
            switch (choice)
            {
                case DialogueChoice.DemoDialogue1:
                    DialogueSystem.Instance.StartDialogue(demoDialogue1);
                    break;

                case DialogueChoice.DemoDialogue2:
                    DialogueSystem.Instance.StartDialogue(demoDialogue2);
                    break;
            }
        }

        private enum DialogueChoice
        {
            DemoDialogue1,
            DemoDialogue2
        }
    }
}