﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using System;
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
        public void StartDialogue(Dialogue dialogue, Action onDialogueFinished = null)
        {
            if (!isHoldingDialogue)
            {
                StartCoroutine(DialogueRoutine(dialogue, onDialogueFinished));
            }
            else
            {
                Debug.LogWarning("A dialogue was started while another was in progress :: this is not intended behaviour!");
            }
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        private IEnumerator DialogueRoutine(Dialogue dialogue, Action onDialogueFinished)
        {
            isHoldingDialogue = true;

            yield return dialogue.Routine(leftCharacterDisplay, rightCharacterDisplay);

            dialogue.Reset();
            onDialogueFinished?.Invoke();

            isHoldingDialogue = false;
        }
    }
}