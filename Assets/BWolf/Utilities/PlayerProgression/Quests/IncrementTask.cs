// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest Task for doing something a set ammount of times</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/IncrementTask")]
    public class IncrementTask : QuestTask
    {
        [Header("Increment Settings")]
        [SerializeField]
        private int count = 0;

        [SerializeField]
        private int goal = 0;

        public override string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// The progress of this task represented by a number between 0 and 1
        /// </summary>
        public override float Progress
        {
            get
            {
                return count / (float)goal;
            }
        }

        /// <summary>
        /// The progress of this task represented as a string
        /// </summary>
        public override string ProgressFormatted
        {
            get { return $"({count}/{goal})"; }
        }

        /// <summary>Increments the count on this task by one</summary>
        public void Increment()
        {
            count++;
            if (count > goal)
            {
                count = goal;
            }
            else
            {
                if (count == goal)
                {
                    Completed();
                }
                SaveToFile();
            }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(IncrementTask)}/{name}";
            if (FileStorageSystem.LoadFromFile(path, out int outValue))
            {
                count = outValue;
            }
        }

        public override void Restore()
        {
            count = 0;
            SaveToFile();

#if UNITY_EDITOR
            //make sure that in the editor, restoring the task marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public override void SaveToFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(IncrementTask)}/{name}";
            FileStorageSystem.SaveToFile(path, count);
        }
    }
}