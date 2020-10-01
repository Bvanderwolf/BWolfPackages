using BWolf.Utilities.PlayerProgression.Quests;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.PlayerProgression
{
    public class PropertyUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject propertyTextPrefab = null;

        private Dictionary<Quest, QuestDisplay> quests = new Dictionary<Quest, QuestDisplay>();

        private void Start()
        {
            QuestManager.Instance.QuestCompleted += OnQuestCompleted;
        }

        private void Update()
        {
            //display active quests on screen
            foreach (Quest quest in QuestManager.Instance.ActiveQuests)
            {
                QuestDisplay display = GetQuestDisplay(quest);
                display.SetDescription(quest.Description);
                display.SetTaskDescription(quest.Tasks);
            }
        }

        private void OnQuestCompleted(Quest quest)
        {
            //destroy quest display and remove quest from quests display if completed
            Destroy(quests[quest].gameObject);
            quests.Remove(quest);
        }

        /// <summary>Returns a quest display for given quest</summary>
        private QuestDisplay GetQuestDisplay(Quest quest)
        {
            if (!quests.ContainsKey(quest))
            {
                quests.Add(quest, Instantiate(propertyTextPrefab, transform).GetComponent<QuestDisplay>());
            }

            return quests[quest];
        }
    }
}