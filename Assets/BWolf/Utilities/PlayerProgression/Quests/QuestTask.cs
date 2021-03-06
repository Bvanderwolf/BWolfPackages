﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>An abstract class for defining a quest task to be part of completing a quest</summary>
    public abstract class QuestTask : ScriptableObject
    {
        [Header("General Settings")]
        [SerializeField]
        protected string description = string.Empty;

        [Header("Saving/Loading")]
        [SerializeField, Tooltip("The path relative to the root path of the app where the quest task data will be stored. " +
            "Use the '.' as directory seperation character")]
        private string folderPath = "ProgressSaves.Quests.QuestTasks";

        public Action Completed;

        /// <summary>
        /// The cross platform path relative to the root path of the app where the quest task data will be stored.
        /// </summary>
        public string FolderPath
        {
            get { return folderPath.Replace('.', System.IO.Path.DirectorySeparatorChar); }
        }

        /// <summary>Is this task finished?</summary>
        public bool IsDone
        {
            get { return Progress == 1.0f; }
        }

        public abstract string Description { get; }
        public abstract float Progress { get; }
        public abstract string ProgressFormatted { get; }

        /// <summary>should restore the quest task</summary>
        [ContextMenu("Restore")]
        public abstract void Restore();

        /// <summary>Should save current value to local storage</summary>
        [ContextMenu("Save")]
        public abstract void SaveToFile();

        /// <summary>Should load current value from local storage</summary>
        [ContextMenu("Load")]
        public abstract void LoadFromFile();
    }
}