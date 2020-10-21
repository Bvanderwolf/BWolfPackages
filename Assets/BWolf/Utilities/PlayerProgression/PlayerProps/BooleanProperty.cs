// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.PlayerProgression.Achievements;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>A boolean value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/BooleanProperty")]
    public class BooleanProperty : PlayerProperty
    {
        [SerializeField]
        private bool booleanValue = false;

        public bool Value
        {
            get { return booleanValue; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(bool newBooleanValue, bool fromSaveFile = false)
        {
            if (booleanValue == newBooleanValue)
            {
                return;
            }

            booleanValue = newBooleanValue;

            foreach (BooleanAchievement achievement in GetAchievements<BooleanAchievement>())
            {
                achievement.UpdateValue(booleanValue);
            }

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateValue(false);
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";
            ProgressFileSystem.SaveProgress(path, booleanValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";

            if (ProgressFileSystem.LoadProgress(path, out bool outValue))
            {
                UpdateValue(outValue, true);
            }
        }
    }
}