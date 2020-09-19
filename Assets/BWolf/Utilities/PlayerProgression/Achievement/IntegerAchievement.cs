// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>An Integer value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/IntegerAchievement")]
    public class IntegerAchievement : ProgressableObject<int>
    {
        public override void UpdateValue(int newValue, bool fromSaveFile = false)
        {
            if (!IsCompleted)
            {
                bool canSave = current != newValue && !fromSaveFile;

                current = Mathf.Clamp(newValue, start, goal);
                progress = Mathf.Clamp01((float)current / goal);

                if (IsCompleted && !fromSaveFile)
                {
                    OnCompletion();
                }

                if (canSave)
                {
                    SaveToFile();
                }
            }
        }
    }
}