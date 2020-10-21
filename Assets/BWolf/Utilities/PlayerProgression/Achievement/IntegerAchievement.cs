// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>An Integer value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/IntegerAchievement")]
    public class IntegerAchievement : Achievement
    {
        [SerializeField]
        private int startValue = 0;

        [SerializeField]
        private int currentValue = 0;

        [SerializeField]
        private int goalValue = 0;

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(IntegerAchievement)}/{name}";

            if (ProgressFileSystem.LoadProgress(path, out int outValue))
            {
                UpdateValue(outValue, false);
            }
        }

        public override void Restore()
        {
            UpdateValue(startValue);
        }

        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(IntegerAchievement)}/{name}";
            ProgressFileSystem.SaveProgress(path, startValue);
        }

        public void UpdateValue(int newValue, bool saveToFile = true)
        {
            if (saveToFile)
            {
                if (currentValue != newValue)
                {
                    currentValue = Mathf.Clamp(newValue, startValue, goalValue);
                    progress = Mathf.Clamp01((float)currentValue / goalValue);

                    SaveToFile();

                    if (IsCompleted)
                    {
                        OnCompletion();
                    }
                }
            }
            else
            {
                currentValue = Mathf.Clamp(newValue, startValue, goalValue);
                progress = Mathf.Clamp01((float)currentValue / goalValue);
            }
        }
    }
}