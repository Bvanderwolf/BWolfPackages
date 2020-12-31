// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>The Player properties stored as a scriptable object</summary>
    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "PlayerProgression/PlayerProps/PropertiesAsset")]
    public class PlayerPropertiesAsset : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Are player properties restored to their original default state when building the application")]
        private bool restoreOnBuild = true;

        [Header("Player Properties")]
        [SerializeField]
        private PlayerProperty[] properties = null;

        public Action<Achievement> AchievementCompleted;

        public const string ASSET_NAME = "PlayerPropertiesAsset";

        /// <summary>Are player properties restored to their original default state when building the application</summary>
        public bool RestoreOnBuild
        {
            get { return restoreOnBuild; }
        }

        private void OnEnable()
        {
            if (properties == null)
            {
                return;
            }

            foreach (PlayerProperty property in properties)
            {
                property.LoadFromFile();
                property.AddListener(OnAchievementCompleted);
            }
        }

        private void OnAchievementCompleted(Achievement achievement)
        {
            AchievementCompleted?.Invoke(achievement);
        }

        /// <summary>Returns a list containing information on all the achievements stored</summary>
        public List<Achievement> GetAllAchievements()
        {
            List<Achievement> info = new List<Achievement>();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Achievements != null)
                {
                    info.AddRange(properties[i].Achievements);
                }
            }

            return info;
        }

        /// <summary>Resets all stored properties</summary>
        [ContextMenu("Restore")]
        public void Restore()
        {
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i].Restore();
            }
        }

        /// <summary>Returns a stored property of type T based on given name</summary>
        public T GetProperty<T>(string propertyName) where T : PlayerProperty
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].name == propertyName)
                {
                    return (T)properties[i];
                }
            }

            return null;
        }

        /// <summary>Returns a stored property based on given name</summary>
        public PlayerProperty GetProperty(string propertyName)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].name == propertyName)
                {
                    return properties[i];
                }
            }

            return null;
        }
    }
}