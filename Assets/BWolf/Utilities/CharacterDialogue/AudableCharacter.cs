// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>A scriptable object representing an audable character to be shown on screen</summary>
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