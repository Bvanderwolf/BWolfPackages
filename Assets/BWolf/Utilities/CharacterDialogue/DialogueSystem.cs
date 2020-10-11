using BWolf.Behaviours.SingletonBehaviours;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    public class DialogueSystem : SingletonBehaviour<DialogueSystem>
    {
        [SerializeField]
        private AudableCharacterDisplay leftCharacterDisplay = null;

        [SerializeField]
        private AudableCharacterDisplay rightCharacterDisplay = null;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }
        }

        public void StartDialogue(Dialogue dialogue)
        {
            StartCoroutine(dialogue.Routine(leftCharacterDisplay, rightCharacterDisplay));
        }
    }
}