// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Pooling")]
        [SerializeField]
        private AudioEmitterPoolSO emitterPool = null;

        [SerializeField]
        private int initialSize = 10;

        [Header("Channels listening to")]
        [SerializeField]
        private AudioRequestChannelSO sfxChannel = null;

        [SerializeField]
        private AudioRequestChannelSO themeChannel = null;

        [Space]
        [SerializeField]
        private AudioCuesEventChannelSO stopChannel = null;

        [SerializeField]
        private AudioCuesEventChannelSO pauseChannel = null;

        [SerializeField]
        private AudioCuesEventChannelSO resumeChannel = null;

        [Header("Audio Profiles")]
        [SerializeField]
        private AudioProfileSO[] profiles = null;

        //A list containg emitters than are playing or pausing audio, which can be stopped, paused or resumed at any point
        private List<AudioEmitter> activeEmitters = new List<AudioEmitter>();

        private void Awake()
        {
            sfxChannel.OnRequestRaised += PlayAudioCue;
            themeChannel.OnRequestRaised += PlayAudioCue;

            stopChannel.OnEventRaised += StopPlayingAudioCues;
            pauseChannel.OnEventRaised += PausePlayingAudioCues;
            resumeChannel.OnEventRaised += ResumePausedAudioCues;

            emitterPool.Prewarm(initialSize);
            emitterPool.SetParent(transform);
        }

        private void Start()
        {
            //audio profile values are loading in start since only on and after start, can audio mixer values be stored when set
            foreach (AudioProfileSO profile in profiles)
            {
                profile.LoadGroupVolumesFromFile();
            }
        }

        /// <summary>
        /// Plays audio based on given configuration, cue object and position
        /// </summary>
        /// <param name="config"></param>
        /// <param name="audioCue"></param>
        /// <param name="position"></param>
        public void PlayAudioCue(AudioConfigurationSO config, AudioCueSO audioCue, Vector3 position = default)
        {
            //play each clip provided by the audio cue
            foreach (AudioClip clip in audioCue.GetClips())
            {
                AudioEmitter emitter = emitterPool.Request();
                emitter.PlayAudioClip(clip, config, audioCue.loop, position);

                activeEmitters.Add(emitter);

                if (!audioCue.loop)
                {
                    //if an emitter is not going to loop, wait for it to finish to return it to the pool
                    emitter.OnAudioFinishedPlaying += ReturnAudioEmitter;
                }
            }
        }

        /// <summary>
        /// Stops active emitters that are not paused from playing, returning them to the pool
        /// </summary>
        /// <param name="audioCues"></param>
        public void StopPlayingAudioCues(AudioCueSO[] audioCues)
        {
            for (int i = 0; i < audioCues.Length; i++)
            {
                for (int j = 0; j < activeEmitters.Count; j++)
                {
                    AudioEmitter emitter = activeEmitters[j];
                    if (emitter.IsPlaying && audioCues[i].IsEmittedBy(emitter))
                    {
                        emitter.Stop();
                        ReturnAudioEmitter(emitter);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Pauses active emitters that are currently playing
        /// </summary>
        /// <param name="audioCues"></param>
        public void PausePlayingAudioCues(AudioCueSO[] audioCues)
        {
            for (int i = 0; i < audioCues.Length; i++)
            {
                for (int j = 0; j < activeEmitters.Count; j++)
                {
                    AudioEmitter emitter = activeEmitters[j];
                    if (emitter.IsPlaying && audioCues[i].IsEmittedBy(emitter))
                    {
                        emitter.Pause();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Resumes actives emitters that are currently paused
        /// </summary>
        /// <param name="audioCues"></param>
        public void ResumePausedAudioCues(AudioCueSO[] audioCues)
        {
            for (int i = 0; i < audioCues.Length; i++)
            {
                for (int j = 0; j < activeEmitters.Count; j++)
                {
                    AudioEmitter emitter = activeEmitters[j];
                    if (!emitter.IsPlaying && audioCues[i].IsEmittedBy(emitter))
                    {
                        emitter.Resume();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes an active emitter from the active emitters list and returns it to the pool
        /// </summary>
        /// <param name="emitter"></param>
        private void ReturnAudioEmitter(AudioEmitter emitter)
        {
            activeEmitters.Remove(emitter);

            emitter.OnAudioFinishedPlaying -= ReturnAudioEmitter;
            emitterPool.Return(emitter);
        }
    }
}