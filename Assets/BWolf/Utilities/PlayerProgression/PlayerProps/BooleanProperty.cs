﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A boolean value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/BooleanProperty")]
    public class BooleanProperty : PlayerProperty
    {
        [SerializeField]
        private bool isTrue = false;

        public bool IsTrue
        {
            get { return isTrue; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(bool newBooleanValue, bool saveToFile = true)
        {
            if (isTrue == newBooleanValue)
            {
                return;
            }

            isTrue = newBooleanValue;

            foreach (BooleanAchievement achievement in GetAchievements<BooleanAchievement>())
            {
                achievement.UpdateValue(isTrue);
            }

            if (saveToFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateValue(default);

#if UNITY_EDITOR
            //make sure that in the editor, restoring the playerproperty marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";
            FileStorageSystem.SaveToFile(path, isTrue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";

            if (FileStorageSystem.LoadFromFile(path, out bool outValue))
            {
                UpdateValue(outValue, false);
            }
        }
    }
}