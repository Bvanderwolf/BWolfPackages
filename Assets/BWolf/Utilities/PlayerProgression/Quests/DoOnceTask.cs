// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.FileStorage;
using System.IO;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest Task for doing something once</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/DoOnceTask")]
    public class DoOnceTask : QuestTask
    {
        [Header("Do Once Settings")]
        [SerializeField]
        private bool isDoneOnce = false;

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
            get { return isDoneOnce ? 1.0f : 0.0f; }
        }

        /// <summary>
        /// The progress of this task represented as a string
        /// </summary>
        public override string ProgressFormatted
        {
            get { return string.Format("({0}/1)", isDoneOnce ? 1 : 0); }
        }

        public override void LoadFromFile()
        {
            string filePath = Path.Combine(FolderPath, name);
            if (FileStorageSystem.LoadFromFile(filePath, out bool outValue))
            {
                isDoneOnce = outValue;
            }
        }

        public override void Restore()
        {
            isDoneOnce = false;
            SaveToFile();

#if UNITY_EDITOR
            //make sure that in the editor, restoring the task marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public override void SaveToFile()
        {
            string filePath = Path.Combine(FolderPath, name);
            FileStorageSystem.SaveToFile(filePath, isDoneOnce);
        }

        /// <summary>Sets this task to done</summary>
        public void SetDoneOnce()
        {
            if (!isDoneOnce)
            {
                isDoneOnce = true;
                Completed();
                SaveToFile();
            }
        }
    }
}