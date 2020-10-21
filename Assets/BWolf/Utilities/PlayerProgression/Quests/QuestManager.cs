// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
// Dependencies: SingletonBehaviours
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>Singleton Manager class providing options to manage all stored quests in the game</summary>
    public class QuestManager : SingletonBehaviour<QuestManager>
    {
        [SerializeField]
        private Quest[] quests = null;

        public List<Quest> ActiveQuests { get; } = new List<Quest>();

        public event Action<Quest> QuestCompleted;

        public Quest[] Quests
        {
            get { return quests; }
        }

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].ActiveStateChanged += OnActiveStateChanged;
                quests[i].Completed += OnQuestCompleted;

                quests[i].LoadActiveStateFromFile();
                quests[i].LoadTasksFromFile();
            }

#if UNITY_EDITOR
            foreach (Quest quest in quests)
            {
                if (quest.IsActive)
                {
                    ActiveQuests.Add(quest);
                }
            }
#endif
        }

        private void Update()
        {
            for (int i = 0; i < ActiveQuests.Count; i++)
            {
                ActiveQuests[i].Update();
            }
        }

        [ContextMenu("ResetProgress")]
        public void ResetProgress()
        {
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].Restore();
            }
        }

        /// <summary>Returns a stored quest with given name. Returns null if no quest is found</summary>
        public Quest GetQuest(string nameOfQuest)
        {
            for (int i = 0; i < quests.Length; i++)
            {
                Quest quest = quests[i];
                if (quest.name == nameOfQuest)
                {
                    return quest;
                }
            }

            return null;
        }

        /// <summary>Called when a quest's active state has changed to add it to or remove it from the activeQuests list</summary>
        private void OnActiveStateChanged(Quest quest, bool value)
        {
            if (value)
            {
                ActiveQuests.Add(quest);
            }
            else
            {
                ActiveQuests.Remove(quest);
            }
        }

        /// <summary>Called when a quest has been completed to notify listeners of this event</summary>
        private void OnQuestCompleted(Quest quest)
        {
            QuestCompleted?.Invoke(quest);
        }
    }
}