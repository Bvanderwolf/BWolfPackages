// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A floating point value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/FloatProperty")]
    public class FloatProperty : PlayerProperty
    {
        [SerializeField]
        private float floatValue = 0.0f;

        public float Value
        {
            get { return floatValue; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(float newfloatValue, bool saveToFile = true)
        {
            if (floatValue == newfloatValue)
            {
                return;
            }

            floatValue = newfloatValue;

            foreach (FloatAchievement achievement in GetAchievements<FloatAchievement>())
            {
                achievement.UpdateValue(floatValue);
            }

            if (saveToFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateValue(0.0f);
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(FloatProperty)}/{name}";
            FileStorageSystem.SaveToFile(path, floatValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(FloatProperty)}/{name}";

            if (FileStorageSystem.LoadFromFile(path, out float outValue))
            {
                UpdateValue(outValue, false);
            }
        }
    }
}