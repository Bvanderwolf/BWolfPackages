// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.ProcessQueues;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>System for managing monologue in the game</summary>
    public class MonologueSystem : BroadcastingProcessManager<Monologue>
    {
        [Header("Settings")]
        [SerializeField]
        private float setupTime = 0.25f;

        [Header("Scene References")]
        [SerializeField]
        private AudableCharacterDisplay display = null;

        [SerializeField]
        private Image backDrop = null;

        [Header("Process Management")]
        [SerializeField]
        private MonologueProcessQueue queue = null;

        [Header("Channels listening to")]
        [SerializeField]
        private MonologueEventChannel requestChannel = null;

        [SerializeField]
        private DialogueEventChannel dialogueEndChannel = null;

        [Header("Channels broadcasting on")]
        [SerializeField]
        private MonologueEventChannel monologueEndChannel = null;

        protected override ProcessEventChannel<Monologue> EventChannel
        {
            get { return monologueEndChannel; }
        }

        protected override ProcessQueue<Monologue> ProcessQueue
        {
            get { return queue; }
        }

        private void Awake()
        {
            requestChannel.OnRequestRaised += OnMonologueRequestRaised;
            dialogueEndChannel.OnRequestRaised += OnDialogueEnded;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void OnMonologueRequestRaised(Monologue monologue)
        {
            if (!Dialogue.AnyActive)
            {
                //if no dialogue is being played, start the monologue process
                StartProcess(monologue);
            }
            else
            {
                //if a dialogue is already being played, the monologue needs to be queued
                queue.Enqueue(monologue);
            }
        }

        private void OnDialogueEnded(Dialogue dialogue)
        {
            if (!Dialogue.AnyActive)
            {
                //if no dialogue is being played after a monologue has ended, check if there is monologue to be played
                CheckForDeque();
            }
        }

        private void ResetDisplayPosition()
        {
            RectTransform rectTransform = (RectTransform)display.transform;
            rectTransform.anchoredPosition = new Vector3(-rectTransform.sizeDelta.x, 0);
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        protected override IEnumerator DoProcess(Monologue monologue)
        {
            backDrop.enabled = true;

            monologue.SetupDisplay(display);

            yield return SetupMonologue();
            yield return monologue.Routine();

            monologue.Restore();

            ResetDisplayPosition();
        }

        protected override void OnCheckedForDeque(Monologue oldInfo, Monologue nextInfo, bool dequeued)
        {
            base.OnCheckedForDeque(oldInfo, nextInfo, dequeued);

            backDrop.enabled = false;
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
    }
}