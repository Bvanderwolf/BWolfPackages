// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.3
//----------------------------------

using BWolf.Utilities.FileStorage;
using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A scriptable object for storing quest information, containing tasks to complete it</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Quests/Quest")]
    public class Quest : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private string description = string.Empty;

        [SerializeField]
        private bool isCompleted = false;

        [SerializeField]
        private bool isActive = false;

        [Header("References")]
        [SerializeField]
        private QuestTask[] tasks = null;

        [Space]
        [SerializeField]
        private Quest requiredQuest = null;

        public event Action<Quest, bool> ActiveStateChanged;

        public event Action<Quest> Completed;

        private const string FOLDER_PATH = "ProgressSaves/Quests/ActiveQuests";

        /// <summary>The stored description given to this quest</summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>Is this quest active?</summary>
        public bool IsActive
        {
            get { return isActive; }
        }

        /// <summary>Can this quest be activated?</summary>
        public bool IsActivatable
        {
            get { return !isActive && FinishedRequiredQuest; }
        }

        /// <summary>Is this quest updatable</summary>
        public bool IsUpdatable
        {
            get { return isActive && !isCompleted; }
        }

        /// <summary>Is this quest completed?</summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        /// <summary>Has player finished the quest that is required for this quest to be activated</summary>
        private bool FinishedRequiredQuest
        {
            get { return requiredQuest == null || requiredQuest.isCompleted; }
        }

        /// <summary>The quest that is required for this quest to be activated</summary>
        public Quest RequiredQuest
        {
            get { return requiredQuest; }
        }

        /// <summary>The stored array of tasks that needs to be done for this quest to be completed</summary>
        public QuestTask[] Tasks
        {
            get { return tasks; }
        }

        private void OnEnable()
        {
            foreach (QuestTask task in tasks)
            {
                task.Completed += CheckCompletion;
            }
        }

        /// <summary>Progression of this quest indicated by a number between 0 and 1</summary>
        public float Progress
        {
            get
            {
                float totalProgress = 0.0f;
                for (int i = 0; i < tasks.Length; i++)
                {
                    totalProgress += tasks[i].Progress;
                }

                return Mathf.Clamp01(totalProgress / tasks.Length);
            }
        }

        /// <summary>Checks the progress of the tasks</summary>
        private void CheckCompletion()
        {
            if (isCompleted)
            {
                return;
            }

            float totalProgress = 0.0f;
            for (int i = 0; i < tasks.Length; i++)
            {
                totalProgress += tasks[i].Progress;
            }

            if (totalProgress >= tasks.Length)
            {
                Completed(this);
                SetActiveInternal(false, true);
                isCompleted = true;
            }
        }

        /// <summary>Returns a Task of Type T based on given name</summary>
        public T GetTask<T>(string nameOfTask) where T : QuestTask
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].name == nameOfTask)
                {
                    return (T)tasks[i];
                }
            }

            return null;
        }

        /// <summary>Returns a Task based on given name</summary>
        public QuestTask GetTask(string nameOfTask)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].name == nameOfTask)
                {
                    return tasks[i];
                }
            }

            return null;
        }

        /// <summary>Loads the task data from local storage</summary>
        public void LoadTasksFromFile()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].LoadFromFile();
            }
        }

        /// <summary>Sets the active state of this quest</summary>
        public void SetActive(bool value)
        {
            SetActiveInternal(value, true);
        }

        /// <summary>Sets the active state of this quest</summary>
        private void SetActiveInternal(bool value, bool saveToFile)
        {
            if (value != isActive)
            {
                isActive = value;
                ActiveStateChanged?.Invoke(this, isActive);

                if (saveToFile)
                {
                    SaveActiveStateToFile();
                }
            }
        }

        /// <summary>Resets this quest, resetting its tasks and setting it to be inactive and incomplete</summary>
        [ContextMenu("Restore")]
        public void Restore()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Restore();
            }

#if UNITY_EDITOR
            //make sure that in the editor, restoring the quest outside of playmode doesn't cause any null references
            if (!UnityEditor.EditorApplication.isPlaying)
                ActiveStateChanged = null;
#endif

            isCompleted = false;
            SetActiveInternal(false, true);

#if UNITY_EDITOR
            //make sure that in the editor, restoring the quest marks it as dirty so it can be saved for version control
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves the active state of this quest to local storage</summary>
        public void SaveActiveStateToFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            FileStorageSystem.SaveToFile(path, isActive);
        }

        /// <summary>Loads the active state of this quest from local storage and assigns its value</summary>
        public void LoadActiveStateFromFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            if (FileStorageSystem.LoadFromFile(path, out bool outValue))
            {
                SetActiveInternal(outValue, false);
            }
        }
    }
}