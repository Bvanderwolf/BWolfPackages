// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>Base class for generic player properties</summary>
    public abstract class PlayerProperty : ScriptableObject
    {
        [SerializeField]
        private Achievement[] achievements = null;

        protected const string FOLDER_NAME = "PlayerProperties";

        /// <summary>Returns an achievement of type T with given name attached to this property</summary>
        public T GetAchievement<T>(string name) where T : Achievement
        {
            for (int i = 0; i < achievements.Length; i++)
            {
                if (achievements[i].name == name)
                {
                    return (T)achievements[i];
                }
            }

            return null;
        }

        /// <summary>Returns a list of Achievements of type T attached to this property</summary>
        public List<T> GetAchievements<T>() where T : Achievement
        {
            List<T> list = new List<T>();
            for (int i = 0; i < achievements.Length; i++)
            {
                if (achievements[i].GetType() == typeof(T))
                {
                    list.Add((T)achievements[i]);
                }
            }

            return list;
        }

        /// <summary>The base Achievements attached to this property</summary>
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
        public abstract void Restore();

        /// <summary>Saves value to local storage</summary>
        protected abstract void SaveToFile();

        /// <summary>Loads value from local storage</summary>
        public abstract void LoadFromFile();
    }
}