// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    /// <summary>Behaviour for displaying the audablecharacter on screen</summary>
    public class AudableCharacterDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image displayImage = null;

        [SerializeField]
        private GameObject textBox = null;

        [SerializeField]
        private Text textComponent = null;

        /// <summary>sprite shown on screen</summary>
        public Sprite sprite
        {
            set { displayImage.sprite = value; }
        }

        /// <summary>text shown in the text box on screen</summary>
        public string text
        {
            set { textComponent.text = value; }
        }

        /// <summary>Either shows or hides the textbox showing the character to be speaking</summary>
        public void SetActive(bool value)
        {
            if (value != textBox.activeInHierarchy)
            {
                textBox.SetActive(value);
            }
        }
    }
}