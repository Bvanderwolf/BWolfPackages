// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using BWolf.Utilities.PlayerProgression.Achievements;
using System.IO;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A boolean value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/BooleanProperty")]
    public class BooleanProperty : PlayerProperty
    {
        [Header("Boolean Settings")]
        [SerializeField]
        private bool value = false;

        public bool Value
        {
            get { return value; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(bool newBooleanValue, bool saveToFile = true)
        {
            if (value == newBooleanValue)
            {
                return;
            }

            value = newBooleanValue;

            foreach (BooleanAchievement achievement in GetAchievements<BooleanAchievement>())
            {
                achievement.UpdateValue(value, saveToFile);
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
            string filePath = Path.Combine(FolderPath, name);
            FileStorageSystem.SaveToFile(filePath, value);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string filePath = Path.Combine(FolderPath, name);

            if (FileStorageSystem.LoadFromFile(filePath, out bool outValue))
            {
                UpdateValue(outValue, false);
            }
        }
    }
}