// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>Utilitie script for working with a sound slider in audio settings</summary>
    [RequireComponent(typeof(Slider))]
    public class AudioVolumeSlider : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private VolumeGroup volumeGroup = VolumeGroup.SFX;

        [Space]
        [SerializeField]
        private AudioConfigurationSO config = null;

        [SerializeField]
        private AudioCueSO audioCue = null;

        [Header("Channels broadcasting on")]
        [SerializeField]
        private AudioRequestChannelSO channel = null;

        [Header("Profile Adjusting")]
        [SerializeField]
        private AudioProfileSO profile = null;

        private Slider slider;

        private const float VALUE_CHANGE_INTERVAL_WAIT = 0.15f;

        private bool waitingForChangeToStop;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private IEnumerator Start()
        {
            yield return null; //wait one frame for the audio profile to set its values

            switch (volumeGroup)
            {
                case VolumeGroup.Music:
                    slider.SetValueWithoutNotify(profile.GetGroupVolume(VolumeGroup.Music));
                    break;

                case VolumeGroup.SFX:
                    slider.SetValueWithoutNotify(profile.GetGroupVolume(VolumeGroup.SFX));
                    break;
            }
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float delta)
        {
            if (volumeGroup != VolumeGroup.SFX)
            {
                //non-SFX values should be updated to the player while changing the slider for accurate feedback
                profile.SetGroupVolume(volumeGroup, slider.value);
            }

            if (!waitingForChangeToStop)
            {
                //if we are not waiting for change to stop, start waiting the slider to stop being changd
                StartCoroutine(WaitForValueChange());
            }
        }

        /// <summary>Returns an enumerator that waits for the slider's value to stop being changed to update the volume afterwards</summary>
        private IEnumerator WaitForValueChange()
        {
            waitingForChangeToStop = true;

            //wait for the slider value to stop being changed
            float change = 0.0f;
            while (change != slider.value)
            {
                change = slider.value;
                yield return new WaitForSeconds(VALUE_CHANGE_INTERVAL_WAIT);
            }

            if (volumeGroup == VolumeGroup.SFX)
            {
                //sfx value is set after value has stopped being changed and feedbacked to the player
                profile.SetGroupVolume(volumeGroup, slider.value);
                channel.RaiseEvent(config, audioCue, Vector3.zero);
            }

            profile.SaveGroupVolumeToFile(volumeGroup);

            waitingForChangeToStop = false;
        }
    }
}