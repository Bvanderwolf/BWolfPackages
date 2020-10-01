using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A behaviour for creating interaction between user and quests, providing quests that can be given</summary>
    public class QuestGiver : MonoBehaviour
    {
        [Header("QuestGiving")]
        [SerializeField]
        protected string description = string.Empty;

        [SerializeField]
        private Quest[] quests = null;

        /// <summary>Gives the player a quests with given name by setting it active</summary>
        public void GiveQuest(string nameOfQuest)
        {
            for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i].name == nameOfQuest)
                {
                    quests[i].SetActive(true);
                }
            }
        }

        /// <summary>Returns a quest with given name</summary>
        public Quest GetQuest(string nameOfQuest)
        {
            for (int i = 0; i < quests.Length; i++)
            {
                if (quests[i].name == nameOfQuest)
                {
                    return quests[i];
                }
            }

            return null;
        }

        /// <summary>Returns a quest of stored quests with given array index</summary>
        public Quest GetQuest(int indexOfQuest)
        {
            if (indexOfQuest >= 0 && indexOfQuest < quests.Length)
            {
                return quests[indexOfQuest];
            }
            else
            {
                return null;
            }
        }
    }
}