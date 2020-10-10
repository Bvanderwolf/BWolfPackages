using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(menuName = "CharacterDialogue/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private AudableCharacter left = null;

        [SerializeField]
        private AudableCharacter right = null;

        [SerializeField]
        private DialoguePart[] dialogue = null;

        public Sprite LeftDisplay
        {
            get { return left.DisplaySprite; }
        }

        public Sprite RightDisplay
        {
            get { return right.DisplaySprite; }
        }

        private int indexAt = -1;

        public bool Continue(out AudableCharacter character)
        {
            indexAt++;

            if (indexAt >= dialogue.Length)
            {
                character = null;
                return false;
            }

            DialoguePart part = dialogue[indexAt];
            character = part.SaidByLeftCharacter ? left : right;
            character.NextLine = part.DialogueText;

            return true;
        }

        public void Reset()
        {
            indexAt = -1;
        }

        [System.Serializable]
        private struct DialoguePart
        {
#pragma warning disable 0649
            public string DialogueText;
            public bool SaidByLeftCharacter;
#pragma warning restore 0649
        }
    }
}