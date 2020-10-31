// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
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

        private Queue<CallbackDialogue> dialogueQueue = new Queue<CallbackDialogue>();

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }
        }

        protected override void OnDestroy()
        {
            StopAllCoroutines();
        }

        /// <summary>Starts a new dialogue if none is already in progress</summary>
        public void StartDialogue(Dialogue dialogue, Action onFinish = null)
        {
            if (!isHoldingDialogue)
            {
                StartCoroutine(DialogueRoutine(dialogue, onFinish));
            }
            else
            {
                dialogueQueue.Enqueue(new CallbackDialogue(dialogue, onFinish));
            }
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        private IEnumerator DialogueRoutine(Dialogue dialogue, Action onDialogueFinished)
        {
            isHoldingDialogue = true;

            yield return dialogue.Routine(leftCharacterDisplay, rightCharacterDisplay);

            dialogue.Restore();
            onDialogueFinished?.Invoke();
            CheckForDeque();
        }

        private void CheckForDeque()
        {
            if (dialogueQueue.Count != 0)
            {
                CallbackDialogue dequedDialogue = dialogueQueue.Dequeue();
                StartCoroutine(DialogueRoutine(dequedDialogue.dialogue, dequedDialogue.callback));
            }
            else
            {
                isHoldingDialogue = false;
            }
        }

        private readonly struct CallbackDialogue
        {
            public readonly Dialogue dialogue;
            public readonly Action callback;

            public CallbackDialogue(Dialogue dialogue, Action callback)
            {
                this.dialogue = dialogue;
                this.callback = callback;
            }
        }
    }
}