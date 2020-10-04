// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.PlayerProps
{
    /// <summary>An Integer value based player property</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/PlayerProps/IntegerProperty")]
    public class IntegerProperty : PlayerProperty
    {
        [SerializeField]
        private int integerValue = 0;

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(int newIntegerValue, bool fromSaveFile = false)
        {
            if (integerValue == newIntegerValue)
            {
                return;
            }

            integerValue = newIntegerValue;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Reset()
        {
            UpdateValue(0);
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(IntegerProperty)}/{name}";
            ProgressFileSystem.SaveProgress(path, integerValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(IntegerProperty)}/{name}";

            if (ProgressFileSystem.LoadProgress(path, out int outValue))
            {
                UpdateValue(outValue, true);
                Debug.LogError(outValue);
            }
        }
    }
}