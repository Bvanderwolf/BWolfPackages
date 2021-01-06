// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>abstract class providing the basis for a progressable object of choosen type</summary>
    public abstract class Achievement : ScriptableObject
    {
        [Header("Achievement Settings")]
        [SerializeField]
        private string description = string.Empty;

        [SerializeField, Range(0.0f, 1.0f)]
        protected float progress = 0.0f;

        [Header("Saving/Loading")]
        [SerializeField, Tooltip("The path relative to the root path of the app where achievement data will be stored. " +
            "Use the '.' as directory seperation character")]
        private string folderPath = "ProgressSaves.Achievements";

        /// <summary>
        /// The cross platform path relative to the root path of the app where achievement data will be stored.
        /// </summary>
        public string FolderPath
        {
            get { return folderPath.Replace('.', System.IO.Path.DirectorySeparatorChar); }
        }

        public Action<Achievement> Completed;

        /// <summary>The stored description given to this achievement</summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>Returns a number between 0 and 1 based on the progress made towards the goal</summary>
        public float Progress
        {
            get { return progress; }
        }

        /// <summary>Returns whether the progression has been completed on this object</summary>
        public bool IsCompleted
        {
            get { return progress == 1.0f; }
        }

        /// <summary>should restore the achiement</summary>
        [ContextMenu("Restore")]
        public abstract void Restore();

        /// <summary>Should save current value to local storage</summary>
        [ContextMenu("Save")]
        protected abstract void SaveToFile();

        /// <summary>Should load current value from local storage</summary>
        [ContextMenu("Load")]
        public abstract void LoadFromFile();
    }
}