using BWolf.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>Singleton Manager class providing options to manage all stored quests in the game</summary>
    public class QuestManager : SingletonBehaviour<QuestManager>
    {
        [Header("Boolean Quests")]
        [SerializeField]
        private BooleanQuest[] booleanQuests = null;

        [Header("Float Quests")]
        [SerializeField]
        private FloatQuest[] floatQuests = null;

        [Header("Integer Quests")]
        [SerializeField]
        private IntegerQuest[] integerQuests = null;

        /// <summary>Called when a quest has been completed</summary>
        public event Action<IProgressInfo> QuestCompleted;

        /// <summary>Provides a list of information on all stored quests</summary>
        public List<IProgressInfo> QuestInfo
        {
            get
            {
                List<IProgressInfo> info = new List<IProgressInfo>();
                info.AddRange(booleanQuests);
                info.AddRange(floatQuests);
                info.AddRange(integerQuests);
                return info;
            }
        }

        private void Awake()
        {
            if (booleanQuests != null)
            {
                for (int i = 0; i < booleanQuests.Length; i++)
                {
                    booleanQuests[i].LoadFromFile();
                    booleanQuests[i].AddListener(OnQuestCompleted);
                }
            }

            if (floatQuests != null)
            {
                for (int i = 0; i < floatQuests.Length; i++)
                {
                    floatQuests[i].LoadFromFile();
                    floatQuests[i].AddListener(OnQuestCompleted);
                }
            }

            if (integerQuests != null)
            {
                for (int i = 0; i < integerQuests.Length; i++)
                {
                    integerQuests[i].LoadFromFile();
                    integerQuests[i].AddListener(OnQuestCompleted);
                }
            }
        }

        private void OnQuestCompleted(IProgressInfo questInfo)
        {
            print($"Quest Completed: {questInfo.Name}");
            QuestCompleted?.Invoke(questInfo);
        }

        /// <summary>Resets all stored Quests</summary>
        [ContextMenu("ResetQuests")]
        public void ResetQuests()
        {
            if (booleanQuests != null)
            {
                for (int i = 0; i < booleanQuests.Length; i++)
                {
                    booleanQuests[i].Reset();
                }
            }

            if (floatQuests != null)
            {
                for (int i = 0; i < floatQuests.Length; i++)
                {
                    floatQuests[i].Reset();
                }
            }

            if (integerQuests != null)
            {
                for (int i = 0; i < integerQuests.Length; i++)
                {
                    integerQuests[i].Reset();
                }
            }
        }
    }
}