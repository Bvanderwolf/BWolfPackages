// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>abstract class providing the basis for a progressable object of choosen type</summary>
    public abstract class Achievement<T> : ScriptableObject, IAchievementInfo
    {
        [SerializeField]
        private string description = string.Empty;

        [SerializeField]
        protected T start = default;

        [SerializeField]
        protected T current;

        [SerializeField]
        protected T goal = default;

        [SerializeField, Range(0.0f, 1.0f)]
        protected float progress = 0.0f;

        private const string FOLDER_NAME = "Achievements";

        private event Action<IAchievementInfo> OnCompletionEvent;

        public string Name
        {
            get { return name; }
        }

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

        /// <summary>OnCompletionEvent invocation method</summary>
        protected void OnCompletion()
        {
            OnCompletionEvent?.Invoke(this);
        }

        /// <summary>Adds a listener for when the goal has been reached for the first time</summary>
        public void AddListener(Action<IAchievementInfo> onCompletion)
        {
            OnCompletionEvent += onCompletion;
        }

        /// <summary>Removes a listener for when the goal has been reached for the first time</summary>
        public void RemoveListener(Action<IAchievementInfo> onCompletion)
        {
            OnCompletionEvent -= onCompletion;
        }

        /// <summary>Resets the progression</summary>
        public void Reset()
        {
            progress = 0.0f;
            UpdateValue(default);
        }

        /// <summary>Saves current value to local storage</summary>
        protected void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{typeof(T).Name}/{name}";
            ProgressFileSystem.SaveProgress(path, current);
        }

        /// <summary>Loads current value from local storage</summary>
        public void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{typeof(T).Name}/{name}";

            if (ProgressFileSystem.LoadProgress(path, out T outValue))
            {
                UpdateValue(outValue, true);
            }
        }

        public abstract void UpdateValue(T newValue, bool fromSaveFile = false);
    }
}