// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using BWolf.Utilities.PlayerProgression.Achievements;
using System.IO;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>An Integer value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/IntegerProperty")]
    public class IntegerProperty : PlayerProperty
    {
        [SerializeField]
        private int integerValue = 0;

        public int Value
        {
            get { return integerValue; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(int newIntegerValue, bool saveToFile = true)
        {
            if (integerValue == newIntegerValue)
            {
                return;
            }

            integerValue = newIntegerValue;

            foreach (IntegerAchievement achievement in GetAchievements<IntegerAchievement>())
            {
                achievement.UpdateValue(integerValue);
            }

            if (saveToFile)
            {
                SaveToFile();
            }
        }

        public void AddValue(int value)
        {
            UpdateValue(integerValue + value);
        }

        public void SubtractValue(int value)
        {
            UpdateValue(integerValue - value);
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
            FileStorageSystem.SaveToFile(filePath, integerValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string filePath = Path.Combine(FolderPath, name);

            if (FileStorageSystem.LoadFromFile(filePath, out int outValue))
            {
                UpdateValue(outValue, false);
            }
        }
    }
}