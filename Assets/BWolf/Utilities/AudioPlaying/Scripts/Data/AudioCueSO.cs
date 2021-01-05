// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>
    /// A collection of audio clips that are played in parallel, and support randomisation.
    /// </summary>
    [CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Cue")]
    public class AudioCueSO : ScriptableObject
    {
        public bool loop = false;

        [SerializeField]
        private AudioClipsGroup[] audioClipsGroups = null;

        public AudioClip[] GetClips()
        {
            int clipCount = audioClipsGroups.Length;
            AudioClip[] clips = new AudioClip[clipCount];

            for (int i = 0; i < clipCount; i++)
            {
                clips[i] = audioClipsGroups[i].GetNextClip();
            }

            return clips;
        }

        /// <summary>
        /// Returns whether this audio cue's clips are contained inside an audio emitter's audio source
        /// </summary>
        /// <param name="emitter"></param>
        /// <returns></returns>
        public bool IsEmittedBy(AudioEmitter emitter)
        {
            foreach (AudioClipsGroup group in audioClipsGroups)
            {
                foreach (AudioClip clip in group.audioClips)
                {
                    if (emitter.ContainsClip(clip))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Represents a group of AudioClips that can be treated as one, and provides automatic randomisation or sequencing based on the <c>SequenceMode</c> value.
        /// </summary>
        [Serializable]
        public class AudioClipsGroup
        {
            public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
            public AudioClip[] audioClips;

            private int nextClipToPlay = -1;
            private int lastClipPlayed = -1;

            /// <summary>
            /// Chooses the next clip in the sequence, either following the order or randomly.
            /// </summary>
            /// <returns>A reference to an AudioClip</returns>
            public AudioClip GetNextClip()
            {
                // Fast out if there is only one clip to play
                if (audioClips.Length == 1)
                    return audioClips[0];

                if (nextClipToPlay == -1)
                {
                    // Index needs to be initialised: 0 if Sequential, random if otherwise
                    nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
                }
                else
                {
                    // Select next clip index based on the appropriate SequenceMode
                    switch (sequenceMode)
                    {
                        case SequenceMode.Random:
                            nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                            break;

                        case SequenceMode.RandomNoImmediateRepeat:
                            do
                            {
                                nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                            } while (nextClipToPlay == lastClipPlayed);
                            break;

                        case SequenceMode.Sequential:
                            nextClipToPlay = (int)Mathf.Repeat(++nextClipToPlay, audioClips.Length);
                            break;
                    }
                }

                lastClipPlayed = nextClipToPlay;

                return audioClips[nextClipToPlay];
            }

            public enum SequenceMode
            {
                Random,
                RandomNoImmediateRepeat,
                Sequential,
            }
        }
    }
}