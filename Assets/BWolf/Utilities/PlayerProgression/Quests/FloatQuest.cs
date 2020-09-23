// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
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