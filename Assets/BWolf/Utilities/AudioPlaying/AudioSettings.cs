// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>ScriptableObject storing music settings used by the player</summary>
    [CreateAssetMenu(menuName = "Audio/Settings")]
    public class AudioSettings : ScriptableObject
    {
        [Range(0.0f, 1.0f)]
        public float ThemeVolume = DEFAULT_VOLUME;

        [Range(0.0f, 1.0f)]
        public float SFXVolume = DEFAULT_VOLUME;

        public const string FILE_PATH = "Music/Settings";
        public const float DEFAULT_VOLUME = 1.0f;
    }
}