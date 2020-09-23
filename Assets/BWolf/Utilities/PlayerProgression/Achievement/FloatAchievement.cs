// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>A floating point value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/FloatAchievement")]
    public class FloatAchievement : ProgressableObject<float>
    {
        public override void UpdateValue(float newValue, bool fromSaveFile = false)
        {
            if (fromSaveFile)
            {
                current = Mathf.Clamp(newValue, start, goal);
                progress = Mathf.Clamp01(current / goal);
            }
            else
            {
                if (current != newValue)
                {
                    current = Mathf.Clamp(newValue, start, goal);
                    progress = Mathf.Clamp01(current / goal);

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