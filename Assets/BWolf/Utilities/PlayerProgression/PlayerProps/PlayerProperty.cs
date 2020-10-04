// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>Base class for generic player properties</summary>
    public abstract class PlayerProperty : ScriptableObject
    {
        [SerializeField]
        private Achievement[] achievements = null;

        protected const string FOLDER_NAME = "PlayerProperties";

        /// <summary>The Achievements attached to this property's value</summary>
        public Achievement[] Achievements
        {
            get
            {
                return achievements;
            }
        }

        /// <summary>Adds a listener to this property for when an achievement has been completed</summary>
        public void AddListener(Action<Achievement> onAchievementCompleted)
        {
            foreach (Achievement progressable in Achievements)
            {
                progressable.AddListener(onAchievementCompleted);
            }
        }

        /// <summary>Removes a listener from this property for when an achievement has been completed</summary>
        public void RemoveListener(Action<Achievement> onAchievementCompleted)
        {
            foreach (Achievement progressable in Achievements)
            {
                progressable.RemoveListener(onAchievementCompleted);
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public abstract void Reset();

        /// <summary>Saves value to local storage</summary>
        protected abstract void SaveToFile();

        /// <summary>Loads value from local storage</summary>
        public abstract void LoadFromFile();
    }
}