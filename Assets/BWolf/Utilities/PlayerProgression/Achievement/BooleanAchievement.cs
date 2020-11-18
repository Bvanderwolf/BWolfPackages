﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>A Boolean value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/BooleanAchievement")]
    public class BooleanAchievement : Achievement
    {
        [SerializeField]
        private bool startValue = false;

        [SerializeField]
        private bool currentValue = false;

        [SerializeField]
        private bool goalValue = true;

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanAchievement)}/{name}";

            if (FileStorageSystem.LoadFromFile(path, out bool outValue))
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
            string path = $"{FOLDER_NAME}/{nameof(BooleanAchievement)}/{name}";
            FileStorageSystem.SaveToFile(path, currentValue);
        }

        public void UpdateValue(bool newValue, bool saveToFile = true)
        {
            if (saveToFile)
            {
                if (currentValue != newValue)
                {
                    currentValue = newValue;
                    progress = currentValue == goalValue ? 1.0f : 0.0f;

                    SaveToFile();

                    if (IsCompleted)
                    {
                        OnCompletion();
                    }
                }
            }
            else
            {
                currentValue = newValue;
                progress = currentValue == goalValue ? 1.0f : 0.0f;
            }
        }
    }
}