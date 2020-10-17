// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest task for accumalating a minimal ammount of value</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/MinimalValueTask")]
    public class MinimalValueTask : QuestTask
    {
        [SerializeField]
        private float currentValue = 0.0f;

        [SerializeField]
        private float minimalValue = 0.0f;

        public override string TaskDescription
        {
            get
            {
                return $"{description} ({currentValue}/{minimalValue})";
            }
        }

        public override float TaskProgres
        {
            get
            {
                return currentValue / minimalValue;
            }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(MinimalValueTask)}/{name}";
            if (ProgressFileSystem.LoadProgress(path, out float outValue))
            {
                currentValue = outValue;
            }
        }

        public override void Restore()
        {
            currentValue = 0;
            SaveToFile();
        }

        public override void SaveToFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(MinimalValueTask)}/{name}";
            ProgressFileSystem.SaveProgress(path, currentValue);
        }

        /// <summary>Updates the current value of this task to given value</summary>
        public void UpdateCurrentValue(float newValue)
        {
            if (newValue != currentValue)
            {
                currentValue = Mathf.Clamp(newValue, 0.0f, minimalValue);
                SaveToFile();
            }
        }
    }
}