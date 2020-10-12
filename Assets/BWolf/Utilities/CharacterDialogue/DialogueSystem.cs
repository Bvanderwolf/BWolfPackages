using BWolf.Behaviours.SingletonBehaviours;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>The Singleton class from where dialogue can be started on 2 character displays</summary>
    public class DialogueSystem : SingletonBehaviour<DialogueSystem>
    {
        [SerializeField]
        private AudableCharacterDisplay leftCharacterDisplay = null;

        [SerializeField]
        private AudableCharacterDisplay rightCharacterDisplay = null;

        private bool isHoldingDialogue;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }
        }

        /// <summary>Starts a new dialogue if none is already in progress</summary>
        public void StartDialogue(Dialogue dialogue)
        {
            if (!isHoldingDialogue)
            {
                StartCoroutine(DialogueRoutine(dialogue));
            }
            else
            {
                Debug.LogWarning("A dialogue was started while another was in progress :: this is not intended behaviour!");
            }
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        private IEnumerator DialogueRoutine(Dialogue dialogue)
        {
            isHoldingDialogue = true;

            yield return dialogue.Routine(leftCharacterDisplay, rightCharacterDisplay);

            dialogue.Reset();
            isHoldingDialogue = false;
        }
    }
}