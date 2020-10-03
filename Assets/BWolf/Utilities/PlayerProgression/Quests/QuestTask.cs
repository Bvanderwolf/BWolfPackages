﻿using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>An abstract class for defining a quest task to be part of completing a quest</summary>
    public abstract class QuestTask : ScriptableObject
    {
        [SerializeField]
        protected string description = string.Empty;

        protected const string FOLDER_PATH = "Quests/QuestTasks";

        public abstract string TaskDescription { get; }
        public abstract float TaskProgres { get; }

        public abstract void LoadFromFile();

        public abstract void SaveToFile();

        public abstract void Reset();
    }
}