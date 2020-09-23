// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
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
            if (fromSaveFile)
            {
                current = Mathf.Clamp(newValue, start, goal);
                progress = Mathf.Clamp01((float)current / goal);
            }
            else
            {
                if (current != newValue)
                {
                    current = Mathf.Clamp(newValue, start, goal);
                    progress = Mathf.Clamp01((float)current / goal);

                    SaveToFile();

                    if (IsCompleted)
                    {
                        OnCompletion();
                    }
                }
            }
        }
    }
}