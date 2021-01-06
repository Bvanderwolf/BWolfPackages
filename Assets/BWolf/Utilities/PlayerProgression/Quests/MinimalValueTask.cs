// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using BWolf.Utilities.FileStorage;
using System.IO;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest task for accumalating a minimal ammount of value</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/MinimalValueTask")]
    public class MinimalValueTask : QuestTask
    {
        [Header("Minimal Value Settings")]
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

        /// <summary>
        /// The progress of this task represented by a number between 0 and 1
        /// </summary>
        public override float Progress
        {
            get
            {
                return currentValue / minimalValue;
            }
        }

        /// <summary>
        /// The progress of this task represented as a string
        /// </summary>
        public override string ProgressFormatted
        {
            get { return $"({currentValue}/{minimalValue})"; }
        }

        public override void LoadFromFile()
        {
            string filePath = Path.Combine(FolderPath, name);
            if (FileStorageSystem.LoadBinary(filePath, out float outValue))
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
            string filePath = Path.Combine(FolderPath, name);
            FileStorageSystem.SaveAsBinary(filePath, currentValue);
        }

        public void AddValue(float value)
        {
            UpdateCurrentValue(currentValue + value);
        }

        /// <summary>Updates the current value of this task to given value</summary>
        public void UpdateCurrentValue(float newValue)
        {
            if (newValue != currentValue)
            {
                float value = Mathf.Clamp(newValue, 0.0f, minimalValue);
                currentValue = value;

                if (currentValue == minimalValue)
                {
                    Completed();
                }

                SaveToFile();
            }
        }
    }
}