// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest Task for doing something once</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/DoOnceTask")]
    public class DoOnceTask : QuestTask
    {
        [SerializeField]
        private bool isDoneOnce = false;

        public override string TaskDescription
        {
            get
            {
                return string.Format("{0} ({1}/1)", description, isDoneOnce ? 1 : 0);
            }
        }

        public override float TaskProgres
        {
            get { return isDoneOnce ? 1.0f : 0.0f; }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(DoOnceTask)}/{name}";
            if (ProgressFileSystem.LoadProgress(path, out bool outValue))
            {
                isDoneOnce = outValue;
            }
        }

        public override void Reset()
        {
            isDoneOnce = false;
            SaveToFile();
        }

        public override void SaveToFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(DoOnceTask)}/{name}";
            ProgressFileSystem.SaveProgress(path, isDoneOnce);
        }

        /// <summary>Sets this task to done</summary>
        public void SetDoneOnce()
        {
            if (!isDoneOnce)
            {
                isDoneOnce = true;
                SaveToFile();
            }
        }
    }
}