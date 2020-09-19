// Created By: Benjamin van der Wolf
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
            if (!IsCompleted)
            {
                bool canSave = current != newValue && !fromSaveFile;

                current = newValue;
                progress = current == goal ? 1.0f : 0.0f;

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