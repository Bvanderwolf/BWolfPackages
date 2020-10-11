using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    [CreateAssetMenu(menuName = "CharacterDialogue/AudableCharacter")]
    public class AudableCharacter : ScriptableObject
    {
        [SerializeField]
        private Sprite displaySprite = null;

        public Sprite DisplaySprite
        {
            get { return displaySprite; }
        }
    }
}