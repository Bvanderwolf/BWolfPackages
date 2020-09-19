// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using BWolf.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>Singleton Manager class providing options to manage all stored player properies in the game</summary>
    public class PropertyManager : SingletonBehaviour<PropertyManager>
    {
        [SerializeField]
        private PlayerProperties propertiesAsset = null;

        public event Action<IProgressInfo> AchievementCompleted;

        private void Awake()
        {
            propertiesAsset.Initialize(OnAchievementCompleted);
        }

        /// <summary>Returns a list of information on all achievements</summary>
        public List<IProgressInfo> AchievementInfo
        {
            get { return propertiesAsset.GetAchievementInfo(); }
        }

        /// <summary>Sets the value of property with given name. Make sure the value type matches the type of property</summary>
        public void SetProperty<T>(string propertyname, T value)
        {
            switch (value)
            {
                case bool booleanValue:
                    propertiesAsset.GetBooleanProperty(propertyname).UpdateValue(booleanValue);
                    break;

                case float floatValue:
                    propertiesAsset.GetFloatProperty(propertyname).UpdateValue(floatValue);
                    break;

                case int integerValue:
                    propertiesAsset.GetIntegerProperty(propertyname).UpdateValue(integerValue);
                    break;

                default:
                    break;
            }
        }

        private void OnAchievementCompleted(IProgressInfo achievementInfo)
        {
            print($"achievement completed: {achievementInfo.Name}");
            AchievementCompleted?.Invoke(achievementInfo);
        }

        /// <summary>Resets all stored player properties</summary>
        [ContextMenu("ResetProgression")]
        public void ResetProgression()
        {
            propertiesAsset.Reset();
        }
    }
}