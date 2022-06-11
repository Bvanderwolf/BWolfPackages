// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.ProcessQueues;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>The class from where dialogue can be started on 2 character displays</summary>
    public class DialogueSystem : BroadcastingProcessManager<Dialogue>
    {
        [Header("Settings")]
        [SerializeField]
        private float setupTime = 0.125f;

        [Header("Scene References")]
        [SerializeField]
        private AudableCharacterDisplay leftCharacterDisplay = null;

        [SerializeField]
        private AudableCharacterDisplay rightCharacterDisplay = null;

        [SerializeField]
        private Image backDrop = null;

        [Header("Process Management")]
        [SerializeField]
        private DialogueProcessQueue queue = null;

        [Header("Channels listening to")]
        [SerializeField]
        private DialogueEventChannel requestChannel = null;

        [SerializeField]
        private MonologueEventChannel monologueEndChannel = null;

        [Header("Channels broadcasting on")]
        [SerializeField]
        private DialogueEventChannel dialogueEndChannel = null;

        protected override ProcessQueue<Dialogue> ProcessQueue
        {
            get { return queue; }
        }

        protected override ProcessEventChannel<Dialogue> EventChannel
        {
            get { return dialogueEndChannel; }
        }

        private void Awake()
        {
            requestChannel.OnEventRaised += OnDialogueRequestRaised;
            monologueEndChannel.OnEventRaised += OnMonologueEnded;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void OnDialogueRequestRaised(Dialogue dialogue)
        {
            if (!Monologue.AnyActive)
            {
                //if no monologue is being played, start the dialogue process
                StartProcess(dialogue);
            }
            else
            {
                //if a monologue is already being played, the dialogue needs to be queued
                queue.Enqueue(dialogue);
            }
        }

        private void OnMonologueEnded(Monologue monologue)
        {
            if (!Monologue.AnyActive)
            {
                //if no monologue is being played after a monologue has ended, check if there is monologue to be played
                CheckForDeque();
            }
        }

        /// <summary>
        /// Places left and right character displays on left and right side of the screen, out of sight
        /// </summary>
        private void ResetDisplayPositions()
        {
            RectTransform leftTransform = (RectTransform)leftCharacterDisplay.transform;
            leftTransform.anchoredPosition = new Vector3(-leftTransform.sizeDelta.x, 0);
            RectTransform rightTransform = (RectTransform)rightCharacterDisplay.transform;
            rightTransform.anchoredPosition = new Vector3(rightTransform.sizeDelta.x, 0);
        }

        /// <summary>Returns an enumerator that waits for the dialogue to finish, resseting it when it has</summary>
        protected override IEnumerator DoProcess(Dialogue dialogue)
        {
            backDrop.enabled = true;

            dialogue.Setup(leftCharacterDisplay, rightCharacterDisplay);

            yield return SetupCharacterDisplays();
            yield return dialogue.Routine();

            dialogue.Restore();

            ResetDisplayPositions();
        }

        protected override void OnCheckedForDeque(Dialogue oldInfo, Dialogue nextInfo, bool dequeued)
        {
            base.OnCheckedForDeque(oldInfo, nextInfo, dequeued);

            backDrop.enabled = false;
        }

        /// <summary>Returns an enumerator that sets up by 2 characters by moving them towards the middle of the screen</summary>
        private IEnumerator SetupCharacterDisplays()
        {
            RectTransform leftTransform = (RectTransform)leftCharacterDisplay.transform;
            RectTransform rightTransform = (RectTransform)rightCharacterDisplay.transform;

            float leftX = -leftTransform.sizeDelta.x;
            float rightX = rightTransform.sizeDelta.x;

            Lerp<float> moveLeftDisplay = new Lerp<float>(leftX, 0, setupTime);
            Lerp<float> moveRightDisplay = new Lerp<float>(rightX, 0, setupTime);

            while (moveLeftDisplay.Continue() && moveRightDisplay.Continue())
            {
                Vector3 newLeft = new Vector3(Mathf.Lerp(moveLeftDisplay.initial, moveLeftDisplay.target, moveLeftDisplay.Percentage), 0);
                leftTransform.anchoredPosition = newLeft;

                Vector3 newRight = new Vector3(Mathf.Lerp(moveRightDisplay.initial, moveRightDisplay.target, moveRightDisplay.Percentage), 0);
                rightTransform.anchoredPosition = newRight;
                yield return null;
            }
        }
    }
}