// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Achievements
{
    /// <summary>abstract class providing the basis for a progressable object of choosen type</summary>
    public abstract class Achievement : ScriptableObject
    {
        [SerializeField]
        private string description = string.Empty;

        [SerializeField, Range(0.0f, 1.0f)]
        protected float progress = 0.0f;

        protected const string FOLDER_NAME = "Achievements";

        private event Action<Achievement> OnCompletionEvent;

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
        public void AddListener(Action<Achievement> onCompletion)
        {
            OnCompletionEvent += onCompletion;
        }

        /// <summary>Removes a listener for when the goal has been reached for the first time</summary>
        public void RemoveListener(Action<Achievement> onCompletion)
        {
            OnCompletionEvent -= onCompletion;
        }

        /// <summary>Resets the progression</summary>
        public abstract void Reset();

        /// <summary>Saves current value to local storage</summary>
        protected abstract void SaveToFile();

        /// <summary>Loads current value from local storage</summary>
        public abstract void LoadFromFile();
    }
}