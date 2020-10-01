using BWolf.Utilities.PlayerProgression.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Examples.PlayerProgression
{
    public class QuestDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text txtDescription = null;

        [SerializeField]
        private Text[] txtTasks = null;

        public void SetDescription(string description)
        {
            txtDescription.text = description;
        }

        public void SetTaskDescription(QuestTask[] tasks)
        {
            for (int i = 0; i < txtTasks.Length; i++)
            {
                txtTasks[i].text = tasks[i].TaskDescription;
            }
        }
    }
}