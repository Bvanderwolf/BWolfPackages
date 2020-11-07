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
    /// <summary>System for managing monologue in the game</summary>
    public class MonologueSystem : SingletonBehaviour<MonologueSystem>
    {
        [Header("Settings")]
        [SerializeField]
        private float setupTime = 0.25f;

        [Header("References")]
        [SerializeField]
        private AudableCharacterDisplay display = null;

        [SerializeField]
        private Image backDrop = null;

        [SerializeField]
        private DialogueSystem dialogueSystem = null;

        public bool IsHoldingMonologue { get; private set; }

        private bool IsHoldingDialogue
        {
            get { return dialogueSystem != null && dialogueSystem.IsHoldingDialogue; }
        }

        public event Action OnMonologueEnd;

        private Queue<CallbackMonologue> monologueQueue = new Queue<CallbackMonologue>();

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            if (dialogueSystem != null)
            {
                dialogueSystem.OnDialogueEnd += CheckForDeque;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StopAllCoroutines();
        }

        /// <summary>Starts a new dialogue if none is already in progress</summary>
        public void StartMonologue(Monologue monologue, Action onFinish = null)
        {
            if (!IsHoldingMonologue && !IsHoldingDialogue)
            {
                StartCoroutine(MonologueRoutine(monologue, onFinish));
            }
            else
            {
                monologueQueue.Enqueue(new CallbackMonologue(monologue, onFinish));
            }
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        private IEnumerator MonologueRoutine(Monologue monologue, Action onFinish = null)
        {
            IsHoldingMonologue = true;
            backDrop.enabled = true;

            monologue.SetupDisplay(display);

            yield return SetupMonologue();
            yield return monologue.Routine();

            monologue.Restore();
            onFinish?.Invoke();

            ResetDisplayPosition();
            CheckForDeque();
            OnMonologueEnd?.Invoke();
        }

        private void CheckForDeque()
        {
            if (monologueQueue.Count != 0 && !IsHoldingDialogue)
            {
                CallbackMonologue dequedMonologue = monologueQueue.Dequeue();
                StartCoroutine(MonologueRoutine(dequedMonologue.monologue, dequedMonologue.callback));
            }
            else
            {
                IsHoldingMonologue = false;
                backDrop.enabled = false;
            }
        }

        /// <summary>Returns an enumerator that sets up by 2 characters by moving them towards the middle of the screen</summary>
        private IEnumerator SetupMonologue()
        {
            RectTransform rectTransform = (RectTransform)display.transform;

            float x = -rectTransform.sizeDelta.x;
            const int HighResWidth = 1920;
            LerpValue<float> moveToMiddle = new LerpValue<float>(x, (x * 0.5f) + (HighResWidth * 0.5f), setupTime);

            while (moveToMiddle.Continue())
            {
                Vector3 towardsMiddle = new Vector3(Mathf.Lerp(moveToMiddle.start, moveToMiddle.end, moveToMiddle.perc), 0);
                rectTransform.anchoredPosition = towardsMiddle;
                yield return null;
            }
        }

        private void ResetDisplayPosition()
        {
            RectTransform rectTransform = (RectTransform)display.transform;
            rectTransform.anchoredPosition = new Vector3(-rectTransform.sizeDelta.x, 0);
        }

        private readonly struct CallbackMonologue
        {
            public readonly Monologue monologue;
            public readonly Action callback;

            public CallbackMonologue(Monologue monologue, Action callback)
            {
                this.monologue = monologue;
                this.callback = callback;
            }
        }
    }
}