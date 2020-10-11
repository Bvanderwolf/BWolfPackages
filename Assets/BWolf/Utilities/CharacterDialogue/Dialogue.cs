using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
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
        private AudableCharacterDisplay activeDisplay;

        public IEnumerator Routine(AudableCharacterDisplay leftDisplay, AudableCharacterDisplay rightDisplay)
        {
            int indexAt = -1;

            leftDisplay.sprite = leftCharacter.DisplaySprite;
            rightDisplay.sprite = rightCharacter.DisplaySprite;

            leftIsActive = startLeft;

            while (Continue(leftDisplay, rightDisplay, ref indexAt))
            {
                yield return WaitForInput();
                yield return null; //wait for update frame so Continue is not called twice
            }
        }

        public bool Continue(AudableCharacterDisplay leftDisplay, AudableCharacterDisplay rightDisplay, ref int indexAt)
        {
            indexAt++;

            if (indexAt == monologues.Length)
            {
                return false;
            }

            activeDisplay = leftIsActive ? leftDisplay : rightDisplay;
            activeDisplay.text = monologues[indexAt].Line;

            if (monologues[indexAt].IsLast)
            {
                leftIsActive = !leftIsActive;
            }

            return true;
        }

        private static IEnumerator WaitForInput()
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }

        [System.Serializable]
        private struct Monologue
        {
#pragma warning disable 0649
            public string Line;
            public bool IsLast;
#pragma warning restore 0649
        }
    }
}