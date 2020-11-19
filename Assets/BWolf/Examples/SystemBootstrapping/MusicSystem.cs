using BWolf.Utilities.SystemBootstrapping;
using UnityEngine;

namespace BWolf.Examples.SystemBootstrapping
{
    public class MusicSystem : SystemBehaviour
    {
        [SerializeField]
        private AudioClip musicClip = null;

        [SerializeField, Range(0.0f, 1.0f)]
        private float volume = 1.0f;

        private void Awake()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = musicClip;
            source.volume = volume;
            source.Play();
        }
    }
}