// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.4
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
        [Header("Achievements")]
        [SerializeField]
        private Achievement[] achievements = null;

        [Header("Saving/Loading")]
        [SerializeField, Tooltip("The path relative to the root path of the app where property data will be stored. " +
            "Use the '.' as directory seperation character")]
        private string folderPath = "ProgressSaves.PlayerProperties";

        /// <summary>
        /// The cross platform path relative to the root path of the app where property data will be stored.
        /// </summary>
        public string FolderPath
        {
            get { return folderPath.Replace('.', System.IO.Path.DirectorySeparatorChar); }
        }

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

        /// <summary>Returns an achievement of with given name attached to this property</summary>
        public Achievement GetAchievement(string name)
        {
            for (int i = 0; i < achievements.Length; i++)
            {
                if (achievements[i].name == name)
                {
                    return achievements[i];
                }
            }

            return null;
        }

        /// <summary>Returns a list of Achievements of type T attached to this property</summary>
        public List<T> GetAchievements<T>() where T : Achievement
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            for (int i = 0; i < achievements.Length; i++)
            {
                if (achievements[i].GetType() == type)
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
            foreach (Achievement achievement in achievements)
            {
                achievement.Completed += onAchievementCompleted;
            }
        }

        /// <summary>Removes a listener from this property for when an achievement has been completed</summary>
        public void RemoveListener(Action<Achievement> onAchievementCompleted)
        {
            foreach (Achievement achievement in Achievements)
            {
                achievement.Completed -= onAchievementCompleted;
            }
        }

        /// <summary>Should reset the value of this property and the achievements attached to it</summary>
        [ContextMenu("Restore")]
        public abstract void Restore();

        /// <summary>Should save current value to local storage</summary>
        [ContextMenu("Save")]
        protected abstract void SaveToFile();

        /// <summary>Should load current value from local storage</summary>
        [ContextMenu("Load")]
        public abstract void LoadFromFile();
    }
}