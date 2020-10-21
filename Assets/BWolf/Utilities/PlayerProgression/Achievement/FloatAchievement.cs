// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>A floating point value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/FloatAchievement")]
    public class FloatAchievement : Achievement
    {
        [SerializeField]
        private float startValue = 0.0f;

        [SerializeField]
        private float currentValue = 0.0f;

        [SerializeField]
        private float goalValue = 0.0f;

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(FloatAchievement)} /{name}";

            if (ProgressFileSystem.LoadProgress(path, out float outValue))
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
            string path = $"{FOLDER_NAME}/{nameof(FloatAchievement)} /{name}";
            ProgressFileSystem.SaveProgress(path, currentValue);
        }

        public void UpdateValue(float newValue, bool saveToFile = true)
        {
            if (saveToFile)
            {
                if (currentValue != newValue)
                {
                    currentValue = Mathf.Clamp(newValue, startValue, goalValue);
                    progress = Mathf.Clamp01(currentValue / goalValue);

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
                progress = Mathf.Clamp01(currentValue / goalValue);
            }
        }
    }
}