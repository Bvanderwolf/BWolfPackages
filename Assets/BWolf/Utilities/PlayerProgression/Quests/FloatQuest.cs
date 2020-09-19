// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>A Floating Point value based Quest</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Quests/FloatQuest")]
    public class FloatQuest : ProgressableObject<float>
    {
        public override void UpdateValue(float newValue, bool fromSaveFile = false)
        {
            if (!IsCompleted)
            {
                bool canSave = current != newValue && !fromSaveFile;

                current = Mathf.Clamp(newValue, start, goal);
                progress = Mathf.Clamp01(current / goal);

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