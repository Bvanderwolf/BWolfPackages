// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>A utility script for making a button play a sound when clicked</summary>
    [RequireComponent(typeof(Button))]
    public class AudableButton : MonoBehaviour
    {
        [Header("AudioSettings")]
        [SerializeField]
        private SFXSound sound = SFXSound.DefaultButtonClick;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlaySound);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            AudioPlayer.Instance.PlaySFXSound(sound);
        }
    }
}