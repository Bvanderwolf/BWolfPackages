// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>
    /// Simple implementation of a MonoBehaviour that is able to request a sound being played at its position by the AudioManager.
    /// It fires an event on an AudioCueEventSO which acts as a channel, that the AudioManager will pick up and play.
    /// </summary>
    public class PositionalAudioRequester : MonoBehaviour
    {
        [Header("Sound")]
        [SerializeField]
        private AudioCueSO audioCue = null;

        [SerializeField]
        private AudioConfigurationSO config = null;

        [SerializeField]
        private bool playOnStart = false;

        [Header("Channel broadcasting on")]
        [SerializeField]
        private AudioRequestChannelSO channel = null;

        private void Start()
        {
            if (playOnStart)
            {
                PlayAudioCue();
            }
        }

        public void PlayAudioCue()
        {
            channel.RaiseEvent(config, audioCue, transform.position);
        }
    }
}