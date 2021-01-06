// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.FileStorage;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.IO;

namespace BWolf.Utilities.AudioPlaying
{
    [CreateAssetMenu(fileName = "newAudioProfile", menuName = "Audio/Profile")]
    public class AudioProfileSO : ScriptableObject
    {
        [Header("Audio Control")]
        [SerializeField]
        private AudioMixer audioMixer = null;

        [Range(0f, 1f)]
        public float masterVolume = 1f;

        [Range(0f, 1f)]
        public float musicVolume = 1f;

        [Range(0f, 1f)]
        public float sfxVolume = 1f;

#if UNITY_EDITOR

        /// <summary>
        /// Re asserts the audio mixer volumes using the stored volume values in this scriptable object. Returns false when not in Play mode
        /// </summary>
        /// <remarks>NOTE: This is only used in the Editor, to debug volumes.
        /// It should be called when any of the variables is changed, and will directly change the values of the volume on the AudioMixer.</remarks>
        public bool ReAssertGroupVolumes()
        {
            if (Application.isPlaying)
            {
                SetGroupVolume(VolumeGroup.Master, masterVolume);
                SetGroupVolume(VolumeGroup.Music, musicVolume);
                SetGroupVolume(VolumeGroup.SFX, sfxVolume);

                return true;
            }
            else
            {
                return false;
            }
        }

#endif

        public void SaveGroupVolumeToFile(VolumeGroup volumeGroup)
        {
            string path = Path.Combine("Audio", "Profiles", $"{name}-{volumeGroup}.txt");
            switch (volumeGroup)
            {
                case VolumeGroup.Master:
                    FileStorageSystem.SaveAsPlainText(path, masterVolume);
                    break;

                case VolumeGroup.Music:
                    FileStorageSystem.SaveAsPlainText(path, musicVolume);
                    break;

                case VolumeGroup.SFX:
                    FileStorageSystem.SaveAsPlainText(path, sfxVolume);
                    break;
            }
        }

        /// <summary>
        /// Saves all volume group values for each volume group to the local storage
        /// </summary>
        public void SaveGroupVolumesToFile()
        {
            VolumeGroup[] volumeGroups = (VolumeGroup[])Enum.GetValues(typeof(VolumeGroup));
            for (int i = 0; i < volumeGroups.Length; i++)
            {
                SaveGroupVolumeToFile(volumeGroups[i]);
            }
        }

        /// <summary>
        /// Loads the volume group values for each volume group and applies these to the audio mixer
        /// </summary>
        /// <remarks>NOTE: This function should only be called on or after MonoBehaviour.Start(). This is because
        /// the AudioMixer can only store values set after after this has happened</remarks>
        public void LoadGroupVolumesFromFile()
        {
            VolumeGroup[] volumeGroups = (VolumeGroup[])Enum.GetValues(typeof(VolumeGroup));
            for (int i = 0; i < volumeGroups.Length; i++)
            {
                LoadGroupVolumeFromFile(volumeGroups[i]);
            }
        }

        private void LoadGroupVolumeFromFile(VolumeGroup volumeGroup)
        {
            string path = Path.Combine("Audio", "Profiles", $"{name}-{volumeGroup}.txt");
            if (FileStorageSystem.LoadPlainText<float>(path, out LoadResult<string> loadResult))
            {
                SetGroupVolume(volumeGroup, float.Parse(loadResult.data));
            }
        }

        /// <summary>
        /// Sets the normalized (Range 0-1) volume value for given volume group
        /// </summary>
        public void SetGroupVolume(VolumeGroup volumeGroup, float normalizedVolume)
        {
            bool volumeSet = audioMixer.SetFloat(volumeGroup.ToString(), NormalizedToMixerValue(normalizedVolume));
            if (volumeSet)
            {
                switch (volumeGroup)
                {
                    case VolumeGroup.Master:
                        masterVolume = normalizedVolume;
                        break;

                    case VolumeGroup.Music:
                        musicVolume = normalizedVolume;
                        break;

                    case VolumeGroup.SFX:
                        sfxVolume = normalizedVolume;
                        break;
                }
            }
            else
            {
                Debug.LogError("The AudioMixer parameter was not found");
            }
        }

        public float GetGroupVolume(VolumeGroup volumeGroup)
        {
            if (audioMixer.GetFloat(volumeGroup.ToString(), out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("The AudioMixer parameter was not found");
                return 0f;
            }
        }

        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations
        /// when using UI sliders normalized format
        private static float MixerValueToNormalized(float mixerValue)
        {
            // We're assuming the range [-80dB to 0dB] becomes [0 to 1]
            return 1f + (mixerValue / 80f);
        }

        private static float NormalizedToMixerValue(float normalizedValue)
        {
            // We're assuming the range [0 to 1] becomes [-80dB to 0dB]
            // This doesn't allow values over 0dB
            return (normalizedValue - 1f) * 80f;
        }
    }

    /// <summary>
    /// Volume groups related to corelating Audio Mixer Group parameters
    /// </summary>
    public enum VolumeGroup
    {
        Master,
        Music,
        SFX
    }
}