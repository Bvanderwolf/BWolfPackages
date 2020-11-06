// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities.FileStorage;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.AudioPlaying
{
    /// <summary>Singleton class for managing audio played in the game</summary>
    public class AudioPlayer : SingletonBehaviour<AudioPlayer>
    {
        [Header("References")]
        [SerializeField]
        private AudioSettings settings = null;

        [Space]
        [SerializeField]
        private SFXSoundContainer[] sfxSounds = null;

        [Space]
        [SerializeField]
        private ThemeSoundContainer[] themeSounds = null;

        private Dictionary<SFXSound, SFXSoundContainer> sfxContainers = new Dictionary<SFXSound, SFXSoundContainer>();
        private Dictionary<ThemeSound, ThemeSoundContainer> themeContainers = new Dictionary<ThemeSound, ThemeSoundContainer>();
        private List<AudioSource> sources = new List<AudioSource>();

        /// <summary>The current SFX volume used ingame</summary>
        public float SFXVolume
        {
            get { return settings.SFXVolume; }
        }

        /// <summary>The current Theme volume used ingame</summary>
        public float ThemeVolume
        {
            get
            {
                return settings.ThemeVolume;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            for (int i = 0; i < sfxSounds.Length; i++)
            {
                SFXSoundContainer container = sfxSounds[i];
                sfxContainers.Add(container.Sound, container);
            }

            for (int i = 0; i < themeSounds.Length; i++)
            {
                ThemeSoundContainer container = themeSounds[i];
                themeContainers.Add(container.Sound, container);
            }

            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                //load music settings in to update volume accordingly
                LoadSettings();
            }

            //start playing default background theme
            PlayThemeSound(ThemeSound.DefaultTheme);
        }

        /// <summary>Saves stored settings to local storage</summary>
        private void SaveSettings()
        {
            FileStorageSystem.SaveAsJsonToFile(AudioSettings.FILE_PATH, settings);
        }

        /// <summary>Tries loading settings from local storage. Uses AudioSettings DefaultVolume as fallback on fail</summary>
        private void LoadSettings()
        {
            if (!FileStorageSystem.LoadAsJsonFromFile(AudioSettings.FILE_PATH, ref settings))
            {
                settings.ThemeVolume = AudioSettings.DEFAULT_VOLUME;
                settings.SFXVolume = AudioSettings.DEFAULT_VOLUME;
                SaveSettings();
            }
        }

        /// <summary>Plays Theme sound of given type</summary>
        public void PlayThemeSound(ThemeSound sound)
        {
            ThemeSoundContainer container = themeContainers[sound];
            PlaySound(GetSource(), container.Clip, true, container.Volume * ThemeVolume);
        }

        /// <summary>Stops theme sound of given type from playing</summary>
        public void StopThemeSound(ThemeSound sound)
        {
            AudioSource source = GetClipSource(themeContainers[sound].Clip);
            if (source != null)
            {
                source.Stop();
            }
            else
            {
                Debug.LogError("Tried stopping theme sound that wasn't playing");
            }
        }

        /// <summary>Plays SFX sound of given type</summary>
        public void PlaySFXSound(SFXSound sound)
        {
            SFXSoundContainer container = sfxContainers[sound];
            PlaySound(GetSource(), container.Clip, false, container.Volume * SFXVolume);
        }

        /// <summary>Stops SFX sound of given type from playing</summary>
        public void StopSFXSound(SFXSound sound)
        {
            AudioSource source = GetClipSource(sfxContainers[sound].Clip);
            if (source != null)
            {
                source.Stop();
            }
            else
            {
                Debug.LogError("Tried stopping sfx sound that wasn't playing");
            }
        }

        /// <summary>Sets the new SFX volume</summary>
        public void UpdateSFXVolume(float newVolume, bool saveToFile)
        {
            if (newVolume != settings.SFXVolume)
            {
                settings.SFXVolume = Mathf.Clamp01(newVolume);

                if (saveToFile)
                {
                    SaveSettings();
                }
            }
        }

        /// <summary>Sets the new volume of given SFX sound type</summary>
        public void UpdateSFXSoundVolume(SFXSound sound, float newVolume)
        {
            SFXSoundContainer container = sfxContainers[sound];
            if (container.Volume != newVolume)
            {
                container.Volume = newVolume;
            }
        }

        /// <summary>Sets the theme volume</summary>
        public void UpdateThemeVolume(float newVolume, bool saveToFile)
        {
            if (newVolume != settings.ThemeVolume)
            {
                settings.ThemeVolume = Mathf.Clamp01(newVolume);

                if (saveToFile)
                {
                    SaveSettings();
                }
            }
        }

        /// <summary>Sets the volume of given theme sound type</summary>
        public void UpdateThemeSoundVolume(ThemeSound sound, float newVolume)
        {
            ThemeSoundContainer container = themeContainers[sound];
            if (container.Volume != newVolume)
            {
                container.Volume = newVolume;
            }
        }

        /// <summary>Plays sound using given source, clip and loop and volume settings</summary>
        private void PlaySound(AudioSource source, AudioClip clip, bool loop, float volume)
        {
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            source.Play();
        }

        /// <summary>Returns an audio source using pooled non-playing sources before creating a new one</summary>
        private AudioSource GetSource()
        {
            AudioSource source;
            for (int i = 0; i < sources.Count; i++)
            {
                source = sources[i];
                if (source.isPlaying)
                {
                    return source;
                }
            }

            source = gameObject.AddComponent<AudioSource>();
            sources.Add(source);
            return source;
        }

        /// <summary>Finds the Audio source this clip is currently attached to. Returns null if none is found</summary>
        private AudioSource GetClipSource(AudioClip clip)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                AudioSource source = sources[i];
                if (source.clip == clip)
                {
                    return source;
                }
            }

            return null;
        }

        [System.Serializable]
        private class SFXSoundContainer
        {
            public SFXSound Sound = SFXSound.DefaultButtonClick;
            public AudioClip Clip = null;

            [Range(0.0f, 1.0f)]
            public float Volume = 1.0f;
        }

        [System.Serializable]
        private class ThemeSoundContainer
        {
            public ThemeSound Sound = ThemeSound.DefaultTheme;
            public AudioClip Clip = null;

            [Range(0.0f, 1.0f)]
            public float Volume = 1.0f;
        }
    }

    /// <summary>The SFX sounds in this game</summary>
    public enum SFXSound
    {
        DefaultButtonClick,
    }

    /// <summary>The Theme sounds in this game</summary>
    public enum ThemeSound
    {
        /// <summary>The one that starts playing when you start the game</summary>
        DefaultTheme,
    }
}