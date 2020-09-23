// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>A Boolean value based Achievement</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Achievements/BooleanAchievement")]
    public class BooleanAchievement : ProgressableObject<bool>
    {
        public override void UpdateValue(bool newValue, bool fromSaveFile = false)
        {
            if (fromSaveFile)
            {
                current = newValue;
                progress = current == goal ? 1.0f : 0.0f;
            }
            else
            {
                if (current != newValue)
                {
                    current = newValue;
                    progress = current == goal ? 1.0f : 0.0f;

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