// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>The class from where dialogue can be started on 2 character displays</summary>
    public class DialogueSystem : SingletonBehaviour<DialogueSystem>
    {
        [Header("Settings")]
        [SerializeField]
        private float setupTime = 0.125f;

        [Header("References")]
        [SerializeField]
        private AudableCharacterDisplay leftCharacterDisplay = null;

        [SerializeField]
        private AudableCharacterDisplay rightCharacterDisplay = null;

        [SerializeField]
        private MonologueSystem monologueSystem = null;

        [SerializeField]
        private Image backDrop = null;

        public bool IsHoldingDialogue { get; private set; }

        public event Action OnDialogueEnd;

        private bool IsHoldingMonologue
        {
            get { return monologueSystem != null && monologueSystem.IsHoldingMonologue; }
        }

        private Queue<CallbackDialogue> dialogueQueue = new Queue<CallbackDialogue>();

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            if (monologueSystem != null)
            {
                monologueSystem.OnMonologueEnd += CheckForDeque;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StopAllCoroutines();
        }

        /// <summary>Starts a new dialogue if none is already in progress</summary>
        public void StartDialogue(Dialogue dialogue, Action onFinish = null)
        {
            if (!IsHoldingDialogue && !IsHoldingMonologue)
            {
                StartCoroutine(DialogueRoutine(dialogue, onFinish));
            }
            else
            {
                dialogueQueue.Enqueue(new CallbackDialogue(dialogue, onFinish));
            }
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        private IEnumerator DialogueRoutine(Dialogue dialogue, Action onFinish = null)
        {
            IsHoldingDialogue = true;
            backDrop.enabled = true;

            dialogue.SetupDisplay(leftCharacterDisplay, rightCharacterDisplay);

            yield return SetupDialogue();
            yield return dialogue.Routine();

            dialogue.Restore();
            onFinish?.Invoke();

            ResetDisplayPositions();
            CheckForDeque();
            OnDialogueEnd?.Invoke();
        }

        private void CheckForDeque()
        {
            if (dialogueQueue.Count != 0 && !IsHoldingMonologue)
            {
                CallbackDialogue dequedDialogue = dialogueQueue.Dequeue();
                StartCoroutine(DialogueRoutine(dequedDialogue.dialogue, dequedDialogue.callback));
            }
            else
            {
                IsHoldingDialogue = false;
                backDrop.enabled = false;
            }
        }

        /// <summary>Returns an enumerator that sets up by 2 characters by moving them towards the middle of the screen</summary>
        private IEnumerator SetupDialogue()
        {
            RectTransform leftTransform = (RectTransform)leftCharacterDisplay.transform;
            RectTransform rightTransform = (RectTransform)rightCharacterDisplay.transform;

            float leftX = -leftTransform.sizeDelta.x;
            float rightX = rightTransform.sizeDelta.x;

            LerpValue<float> moveLeftDisplay = new LerpValue<float>(leftX, 0, setupTime);
            LerpValue<float> moveRightDisplay = new LerpValue<float>(rightX, 0, setupTime);

            while (moveLeftDisplay.Continue() && moveRightDisplay.Continue())
            {
                Vector3 newLeft = new Vector3(Mathf.Lerp(moveLeftDisplay.start, moveLeftDisplay.end, moveLeftDisplay.perc), 0);
                leftTransform.anchoredPosition = newLeft;

                Vector3 newRight = new Vector3(Mathf.Lerp(moveRightDisplay.start, moveRightDisplay.end, moveRightDisplay.perc), 0);
                rightTransform.anchoredPosition = newRight;
                yield return null;
            }
        }

        private void ResetDisplayPositions()
        {
            RectTransform leftTransform = (RectTransform)leftCharacterDisplay.transform;
            leftTransform.anchoredPosition = new Vector3(-leftTransform.sizeDelta.x, 0);
            RectTransform rightTransform = (RectTransform)rightCharacterDisplay.transform;
            rightTransform.anchoredPosition = new Vector3(rightTransform.sizeDelta.x, 0);
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