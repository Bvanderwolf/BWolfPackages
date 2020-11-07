// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>A scriptableobject representing a monologue done by one audable character</summary>
    [CreateAssetMenu(menuName = "CharacterDialogue/Monologue")]
    public class Monologue : ScriptableObject
    {
        [SerializeField]
        private AudableCharacter character = null;

        [SerializeField]
        private string[] lines = null;

        public Action<int> OnContinue;

        private AudableCharacterDisplay display;

        public void SetupDisplay(AudableCharacterDisplay display)
        {
            this.display = display;
            this.display.sprite = character.DisplaySprite;
        }

        /// <summary>Returns an Enumerator that plays out the monologues by each character</summary>
        public IEnumerator Routine()
        {
            int indexAt = -1;

            this.display.SetActive(true);

            while (Continue(display, ref indexAt))
            {
                OnContinue?.Invoke(indexAt);

                yield return WaitForInput();
                yield return null; //wait for update frame so Continue is not called twice
            }
        }

        /// <summary>Tries Progresses the dialogue by switching display if necessary and dislay the next line on screen. Returns false if the end of monologues has been reached.</summary>
        public bool Continue(AudableCharacterDisplay display, ref int indexAt)
        {
            indexAt++;

            if (indexAt == lines.Length)
            {
                return false;
            }

            display.text = lines[indexAt];

            return true;
        }

        /// <summary>Resets the dialogue state</summary>
        public void Restore()
        {
            display?.SetActive(false);
            display = null;
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
    }
}