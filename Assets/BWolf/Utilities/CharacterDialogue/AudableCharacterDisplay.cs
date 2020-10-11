using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    public class AudableCharacterDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image displayImage = null;

        [SerializeField]
        private Text textBox = null;

        public Sprite sprite
        {
            set { displayImage.sprite = value; }
        }

        public string text
        {
            set { textBox.text = value; }
        }
    }
}