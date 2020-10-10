using BWolf.Behaviours.SingletonBehaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.CharacterDialogue
{
    public class DialogueSystem : SingletonBehaviour<DialogueSystem>
    {
        [SerializeField]
        private AudableCharacterDisplay leftCharacterDisplay;

        [SerializeField]
        private AudableCharacterDisplay rightCharacterDisplay;

        public void StartDialogue(Dialogue dialogue)
        {
            leftCharacterDisplay.ImgDisplay.sprite = dialogue.LeftDisplay;
            rightCharacterDisplay.ImgDisplay.sprite = dialogue.RightDisplay;

            StartCoroutine(DialogueRoutine(dialogue));
        }

        private IEnumerator DialogueRoutine(Dialogue dialogue)
        {
            while (dialogue.Continue(out AudableCharacter character))
            {
                while (!Input.GetMouseButton(0) && Input.touchCount == 0)
                {
                    yield return null;
                }
            }
        }

        [System.Serializable]
        private struct AudableCharacterDisplay
        {
#pragma warning disable 0649
            public Image ImgDisplay;
            public Text DialogueDisplay;
#pragma warning restore 0649
        }
    }
}