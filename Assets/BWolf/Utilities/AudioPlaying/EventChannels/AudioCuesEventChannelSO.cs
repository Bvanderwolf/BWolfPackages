// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    [CreateAssetMenu(fileName = "newAudioCuesEventChannel", menuName = "SO Event Channels/Audio Cues")]
    public class AudioCuesEventChannelSO : ScriptableObject
    {
        public Action<AudioCueSO[]> OnEventRaised;

        public void RaiseEvent(AudioCueSO[] audioCues)
        {
            OnEventRaised?.Invoke(audioCues);
        }
    }
}