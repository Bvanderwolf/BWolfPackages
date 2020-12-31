// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>The Player properties stored as a scriptable object</summary>
    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "PlayerProgression/Quests/QuestAsset")]
    public class QuestAsset : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Are Quests restored to their original default state when building the application")]
        private bool restoreOnBuild = true;

        [Header("Quests")]
        [SerializeField]
        private Quest[] quests = null;

        [Space]
        public List<Quest> ActiveQuests = new List<Quest>();

        public Action<Quest> OnQuestCompletedEvent;

        public const string ASSET_NAME = "QuestAsset";

        /// <summary>All quests stored in this asset</summary>
        public Quest[] Quests
        {
            get { return quests; }
        }

        private void OnEnable()
        {
            //hookup events and load save data afterwards
            foreach (Quest quest in quests)
            {
                quest.ActiveStateChanged += OnActiveStateChanged;
                quest.Completed += OnQuestCompleted;

                quest.LoadActiveStateFromFile();
                quest.LoadTasksFromFile();
            }

#if UNITY_EDITOR
            //in the editor, active quests save their state which means they won't fire the ActiveStateChanged event
            //so we add them manually to the active quests list on enable
            foreach (Quest quest in quests)
            {
                if (quest.IsActive && !ActiveQuests.Contains(quest))
                {
                    ActiveQuests.Add(quest);
                }
            }
#endif
        }

        private void OnQuestCompleted(Quest quest)
        {
            OnQuestCompletedEvent?.Invoke(quest);
        }

        /// <summary>Are Quests restored to their original default state when building the application</summary>
        public bool RestoreOnBuild
        {
            get { return restoreOnBuild; }
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

        /// <summary>Resets the progression by restoring all quests to their original default state</summary>
        public void Restore()
        {
            ActiveQuests.Clear();

            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].Restore();
            }
        }
    }
}