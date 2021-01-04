// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;
using System;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>
    /// Event on which AudioCue components send a message to play SFX and music. The AudioManager listens to these events, and actually plays the sound.
    /// </summary>
    [CreateAssetMenu(menuName = "SO Event Channels/AudioRequest")]
    public class AudioRequestChannelSO : ScriptableObject
    {
        public Action<AudioConfigurationSO, AudioCueSO, Vector3> OnRequestRaised;

        public void RaiseEvent(AudioConfigurationSO config, AudioCueSO cue, Vector3 position)
        {
            if (OnRequestRaised != null)
            {
                OnRequestRaised(config, cue, position);
            }
            else
            {
                Debug.LogWarning("An AudioCue was requested, but nobody picked it up. " +
                "Check why there is no AudioManager already loaded, " +
                "and make sure it's listening on this AudioCue Event channel.");
            }
        }
    }
}