// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>The Player properties stored as a scriptable object</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/PropertiesAsset")]
    public class PlayerProperties : ScriptableObject
    {
        [SerializeField]
        private PlayerProperty[] properties = null;

        /// <summary>Initializes properties, loading their values from local storage and listening for achievement completion using given listener</summary>
        public void Initialize(Action<Achievement> onAchievementCompleted)
        {
            if (properties != null)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i].LoadFromFile();
                    properties[i].AddListener(onAchievementCompleted);
                }
            }
        }

        /// <summary>Returns a list containing information on all the achievements stored</summary>
        public List<Achievement> GetAchievementInfo()
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
        public void Reset()
        {
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i].Restore();
            }
        }

        /// <summary>Returns a stored integer property based on given name</summary>
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
    }
}