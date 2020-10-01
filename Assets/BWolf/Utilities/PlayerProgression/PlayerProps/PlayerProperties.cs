// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
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
        private BooleanProperty[] booleanProperties = null;

        [SerializeField]
        private FloatProperty[] floatProperties = null;

        [SerializeField]
        private IntegerProperty[] integerProperties = null;

        /// <summary>Initializes properties, loading their values from local storage and listening for achievement completion using given listener</summary>
        public void Initialize(Action<IAchievementInfo> onAchievementCompleted)
        {
            if (booleanProperties != null)
            {
                for (int i = 0; i < booleanProperties.Length; i++)
                {
                    booleanProperties[i].LoadFromFile();
                    booleanProperties[i].AddListener(onAchievementCompleted);
                }
            }

            if (floatProperties != null)
            {
                for (int i = 0; i < floatProperties.Length; i++)
                {
                    floatProperties[i].LoadFromFile();
                    floatProperties[i].AddListener(onAchievementCompleted);
                }
            }

            if (integerProperties != null)
            {
                for (int i = 0; i < integerProperties.Length; i++)
                {
                    integerProperties[i].LoadFromFile();
                    integerProperties[i].AddListener(onAchievementCompleted);
                }
            }
        }

        /// <summary>Returns a list containing information on all the achievements stored</summary>
        public List<IAchievementInfo> GetAchievementInfo()
        {
            List<IAchievementInfo> info = new List<IAchievementInfo>();

            if (booleanProperties != null)
            {
                for (int i = 0; i < booleanProperties.Length; i++)
                    if (booleanProperties[i].Achievements != null)
                        info.AddRange(booleanProperties[i].Achievements);
            }

            if (floatProperties != null)
            {
                for (int i = 0; i < floatProperties.Length; i++)
                    if (floatProperties[i].Achievements != null)
                        info.AddRange(floatProperties[i].Achievements);
            }

            if (integerProperties != null)
            {
                for (int i = 0; i < integerProperties.Length; i++)
                    if (integerProperties[i].Achievements != null)
                        info.AddRange(integerProperties[i].Achievements);
            }

            return info;
        }

        /// <summary>Resets all stored properties</summary>
        public void Reset()
        {
            if (booleanProperties != null)
            {
                for (int i = 0; i < booleanProperties.Length; i++)
                    booleanProperties[i].Reset();
            }

            if (floatProperties != null)
            {
                for (int i = 0; i < floatProperties.Length; i++)
                    floatProperties[i].Reset();
            }

            if (integerProperties != null)
            {
                for (int i = 0; i < integerProperties.Length; i++)
                    integerProperties[i].Reset();
            }
        }

        /// <summary>Returns a stored boolean property based on given name</summary>
        public BooleanProperty GetBooleanProperty(string propertyName)
        {
            for (int i = 0; i < booleanProperties.Length; i++)
            {
                if (booleanProperties[i].name == propertyName)
                {
                    return booleanProperties[i];
                }
            }

            return null;
        }

        /// <summary>Returns a stored floating point property based on given name</summary>
        public FloatProperty GetFloatProperty(string propertyName)
        {
            for (int i = 0; i < floatProperties.Length; i++)
            {
                if (floatProperties[i].name == propertyName)
                {
                    return floatProperties[i];
                }
            }

            return null;
        }

        /// <summary>Returns a stored integer property based on given name</summary>
        public IntegerProperty GetIntegerProperty(string propertyName)
        {
            for (int i = 0; i < integerProperties.Length; i++)
            {
                if (integerProperties[i].name == propertyName)
                {
                    return integerProperties[i];
                }
            }

            return null;
        }
    }
}