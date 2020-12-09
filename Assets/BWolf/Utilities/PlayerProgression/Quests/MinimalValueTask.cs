// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
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

        public override string Description
        {
            get
            {
                return description;
            }
        }

        public override float Progress
        {
            get
            {
                return currentValue / minimalValue;
            }
        }

        public override string ProgressFormatted
        {
            get { return $"({currentValue}/{minimalValue})"; }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(MinimalValueTask)}/{name}";
            if (FileStorageSystem.LoadFromFile(path, out float outValue))
            {
                currentValue = outValue;
            }
        }

        public override void Restore()
        {
            currentValue = 0;
            SaveToFile();

#if UNITY_EDITOR
            //make sure that in the editor, restoring the task marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public override void SaveToFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(MinimalValueTask)}/{name}";
            FileStorageSystem.SaveToFile(path, currentValue);
        }

        /// <summary>Updates the current value of this task to given value</summary>
        public void UpdateCurrentValue(float newValue)
        {
            if (newValue != currentValue)
            {
                float value = Mathf.Clamp(newValue, 0.0f, minimalValue);
                if (currentValue != minimalValue && value == minimalValue)
                {
                    Completed?.Invoke(this);
                }

                currentValue = value;
                SaveToFile();
            }
        }
    }
}