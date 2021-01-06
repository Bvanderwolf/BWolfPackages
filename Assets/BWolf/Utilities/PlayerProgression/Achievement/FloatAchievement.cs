// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using System.IO;
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
            string filePath = Path.Combine(FolderPath, name);

            if (FileStorageSystem.LoadFromFile(filePath, out float outValue))
            {
                UpdateValue(outValue, false);
            }
        }

        public override void Restore()
        {
            UpdateValue(startValue);

#if UNITY_EDITOR
            //make sure that in the editor, restoring the achievement marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        protected override void SaveToFile()
        {
            string filePath = Path.Combine(FolderPath, name);
            FileStorageSystem.SaveToFile(filePath, currentValue);
        }

        public void UpdateValue(float newValue, bool saveToFile = true)
        {
            if (newValue < startValue || newValue > goalValue)
            {
                return;
            }

            if (saveToFile)
            {
                if (currentValue != newValue)
                {
                    currentValue = Mathf.Clamp(newValue, startValue, goalValue);
                    progress = Mathf.Clamp01(currentValue / goalValue);

                    SaveToFile();

                    if (IsCompleted)
                    {
                        Completed(this);
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