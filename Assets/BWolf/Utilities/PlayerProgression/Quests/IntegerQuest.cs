﻿// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>An Integer value based Quest</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Quests/IntegerQuest")]
    public class IntegerQuest : ProgressableObject<int>
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