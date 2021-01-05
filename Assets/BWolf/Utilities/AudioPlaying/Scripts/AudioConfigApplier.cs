// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioConfigApplier : MonoBehaviour
    {
        [SerializeField]
        private AudioConfigurationSO config = null;

        private AudioSource source;

        private void Start()
        {
            ApplyConfigToSource();
        }

        private void OnValidate()
        {
            ApplyConfigToSource();
        }

        private void ApplyConfigToSource()
        {
            if (config != null)
            {
                config.ApplyToSource(GetComponent<AudioSource>());
            }
        }
    }
}