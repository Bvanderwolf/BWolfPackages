// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>A scriptableobject representing the dialogue between a character on the left side of the screen and one on the right</summary>
    [CreateAssetMenu(menuName = "CharacterDialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private bool startLeft = true;

        [SerializeField]
        private AudableCharacter leftCharacter = null;

        [SerializeField]
        private AudableCharacter rightCharacter = null;

        [SerializeField]
        private Monologue[] monologues = null;

        private bool leftIsActive;
        private bool hasSwitched;

        private AudableCharacterDisplay activeDisplay;
        private AudableCharacterDisplay leftDisplay;
        private AudableCharacterDisplay rightDisplay;

        public Action<int> OnContinue;

        /// <summary>
        /// Indicates whether any dialogue is being played right now
        /// </summary>
        public static bool AnyActive { get; private set; }

        /// <summary>
        /// prepares the dialogue to be played using given audable character displays
        /// </summary>
        /// <param name="leftDisplay"></param>
        /// <param name="rightDisplay"></param>
        public void Setup(AudableCharacterDisplay leftDisplay, AudableCharacterDisplay rightDisplay)
        {
            AnyActive = true;

            this.leftDisplay = leftDisplay;
            this.rightDisplay = rightDisplay;

            leftDisplay.SetActive(false);
            rightDisplay.SetActive(false);

            leftDisplay.sprite = leftCharacter.DisplaySprite;
            rightDisplay.sprite = rightCharacter.DisplaySprite;

            leftIsActive = startLeft;
            activeDisplay = leftIsActive ? leftDisplay : rightDisplay;
        }

        /// <summary>Returns an Enumerator that plays out the monologues by each character</summary>
        public IEnumerator Routine()
        {
            int indexAt = -1;

            activeDisplay.SetActive(true);

            while (Continue(ref indexAt))
            {
                OnContinue?.Invoke(indexAt);

                yield return WaitForInput();
                yield return null; //wait for update frame so Continue is not called twice
            }
        }

        /// <summary>Tries Progresses the dialogue by switching display if necessary and dislay the next line on screen. Returns false if the end of monologues has been reached.</summary>
        public bool Continue(ref int indexAt)
        {
            indexAt++;

            if (indexAt == monologues.Length)
            {
                return false;
            }

            if (hasSwitched)
            {
                activeDisplay.SetActive(false);
                activeDisplay = leftIsActive ? leftDisplay : rightDisplay;
                activeDisplay.SetActive(true);
                hasSwitched = false;
            }

            activeDisplay.text = monologues[indexAt].Line;

            if (monologues[indexAt].IsLast)
            {
                leftIsActive = !leftIsActive;
                hasSwitched = true;
            }

            return true;
        }

        /// <summary>Resets the dialogue state</summary>
        public void Restore()
        {
            hasSwitched = false;

            activeDisplay?.SetActive(false);
            activeDisplay = null;

            AnyActive = false;
        }

        private void OnDisable()
        {
            Restore();
        }

        /// <summary>Returns an enumerator that waits for the mouse button to be pressed</summary>
        private static IEnumerator WaitForInput()
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }

        [Serializable]
        private struct Monologue
        {
#pragma warning disable 0649
            public string Line;
            public bool IsLast;
#pragma warning restore 0649
        }
    }
}