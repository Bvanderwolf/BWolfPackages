﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>Base class for generic player properties</summary>
    public abstract class PlayerProperty<T> : ScriptableObject
    {
        [SerializeField]
        private T _value = default;

        private const string FOLDER_NAME = "PlayerProperties";

        /// <summary>The Achievements attached to this property's value</summary>
        public abstract Achievement<T>[] Achievements { get; }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(T newValue, bool fromSaveFile = false)
        {
            if (Equals(newValue, _value))
            {
                return;
            }

            _value = newValue;

            for (int i = 0; i < Achievements.Length; i++)
            {
                Achievements[i].UpdateValue(_value, fromSaveFile);
            }

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Adds a listener to this property for when an achievement has been completed</summary>
        public void AddListener(Action<IAchievementInfo> onAchievementCompleted)
        {
            foreach (Achievement<T> progressable in Achievements)
            {
                progressable.AddListener(onAchievementCompleted);
            }
        }

        /// <summary>Removes a listener from this property for when an achievement has been completed</summary>
        public void RemoveListener(Action<IAchievementInfo> onAchievementCompleted)
        {
            foreach (Achievement<T> progressable in Achievements)
            {
                progressable.RemoveListener(onAchievementCompleted);
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public void Reset()
        {
            UpdateValue(default);
        }

        /// <summary>Saves value to local storage</summary>
        private void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{typeof(T).Name}/{name}";
            ProgressFileSystem.SaveProgress(path, _value);
        }

        /// <summary>Loads value from local storage</summary>
        public void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{typeof(T).Name}/{name}";

            if (ProgressFileSystem.LoadProgress(path, out T outValue))
            {
                UpdateValue(outValue, true);
            }
        }
    }
}