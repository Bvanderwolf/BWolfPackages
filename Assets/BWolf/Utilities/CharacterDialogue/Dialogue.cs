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

        public Action<int> OnContinue;

        /// <summary>Returns an Enumerator that plays out the monologues by each character</summary>
        public IEnumerator Routine(AudableCharacterDisplay leftDisplay, AudableCharacterDisplay rightDisplay)
        {
            int indexAt = -1;

            leftDisplay.sprite = leftCharacter.DisplaySprite;
            leftDisplay.SetActive(false);

            rightDisplay.sprite = rightCharacter.DisplaySprite;
            rightDisplay.SetActive(false);

            leftIsActive = startLeft;
            activeDisplay = leftIsActive ? leftDisplay : rightDisplay;
            activeDisplay.SetActive(true);

            while (Continue(leftDisplay, rightDisplay, ref indexAt))
            {
                OnContinue?.Invoke(indexAt);

                yield return WaitForInput();
                yield return null; //wait for update frame so Continue is not called twice
            }
        }

        /// <summary>Tries Progresses the dialogue by switching display if necessary and dislay the next line on screen. Returns false if the end of monologues has been reached.</summary>
        public bool Continue(AudableCharacterDisplay leftDisplay, AudableCharacterDisplay rightDisplay, ref int indexAt)
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