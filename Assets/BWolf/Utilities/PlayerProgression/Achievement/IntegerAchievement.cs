// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using System.IO;
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
            string filePath = Path.Combine(FolderPath, name);

            if (FileStorageSystem.LoadFromFile(filePath, out int outValue))
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
            FileStorageSystem.SaveToFile(filePath, startValue);
        }

        public void UpdateValue(int newValue, bool saveToFile = true)
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
                    progress = Mathf.Clamp01((float)currentValue / goalValue);

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
                progress = Mathf.Clamp01((float)currentValue / goalValue);
            }
        }
    }
}