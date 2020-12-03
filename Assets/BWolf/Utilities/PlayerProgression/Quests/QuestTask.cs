// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>An abstract class for defining a quest task to be part of completing a quest</summary>
    public abstract class QuestTask : ScriptableObject
    {
        [SerializeField]
        protected string description = string.Empty;

        protected const string FOLDER_PATH = "ProgressSaves/Quests/QuestTasks";

        /// <summary>Is this task finished?</summary>
        public bool IsDone
        {
            get { return Progress == 1.0f; }
        }

        public abstract string Description { get; }
        public abstract float Progress { get; }
        public abstract string ProgressFormatted { get; }

        public abstract void LoadFromFile();

        public abstract void SaveToFile();

        public abstract void Restore();
    }
}