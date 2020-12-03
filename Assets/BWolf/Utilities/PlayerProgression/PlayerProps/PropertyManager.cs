﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
// Dependencies: SingletonBehaviours
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities.PlayerProgression.Achievements;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>Singleton Manager class providing options to manage all stored player properies in the game</summary>
    public class PropertyManager : SingletonBehaviour<PropertyManager>
    {
        [SerializeField]
        private PlayerPropertiesAsset propertiesAsset = null;

        public event Action<Achievement> AchievementCompleted;

        protected override void Awake()
        {
            base.Awake();

            propertiesAsset.Initialize(OnAchievementCompleted);
        }

        /// <summary>All stored achievements</summary>
        public List<Achievement> AchievementInfo
        {
            get { return propertiesAsset.GetAllAchievements(); }
        }

        /// <summary>Gets a property of type T with given name. Make sure type T matches the type of property</summary>
        public T GetProperty<T>(string propertyname) where T : PlayerProperty
        {
            return propertiesAsset.GetProperty<T>(propertyname);
        }

        /// <summary>Gets a property with given name. Make sure type T matches the type of property</summary>
        public PlayerProperty GetProperty(string propertyname)
        {
            return propertiesAsset.GetProperty(propertyname);
        }

        private void OnAchievementCompleted(Achievement achievementInfo)
        {
            AchievementCompleted?.Invoke(achievementInfo);
        }

        /// <summary>Resets all stored player properties</summary>
        [ContextMenu("ResetProgression")]
        public void ResetProgression()
        {
            propertiesAsset.Restore();
        }
    }
}