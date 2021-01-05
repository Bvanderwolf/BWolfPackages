// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEmitter : MonoBehaviour
    {
        private AudioSource source;

        public event Action<AudioEmitter> OnAudioFinishedPlaying;

        /// <summary>
        /// Returns whether this audio emmitter is playing audio
        /// </summary>
        public bool IsPlaying
        {
            get { return source.isPlaying; }
        }

        /// <summary>
        /// Returns whether this audio emmiter is looping audio
        /// </summary>
        public bool IsLooping
        {
            get { return source.loop; }
        }

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
        }

        /// <summary>
        /// Instructs the AudioSource to play a single clip, with optional looping, in a position in 3D space.
        /// </summary>
        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO config, bool loop, Vector3 position = default)
        {
            transform.position = position;

            config.ApplyToSource(source);

            source.clip = clip;
            source.loop = loop;
            source.Play();

            if (!loop)
            {
                StartCoroutine(WaitForAudioFinish(clip.length));
            }
        }

        public bool ContainsClip(AudioClip clip)
        {
            return source.clip == clip;
        }

        public void Resume()
        {
            source.Play();
        }

        public void Pause()
        {
            source.Pause();
        }

        public void Stop()
        {
            source.Stop();
        }

        private IEnumerator WaitForAudioFinish(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            OnAudioFinishedPlaying(this);
        }
    }
}