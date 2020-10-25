// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

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

        public const string ASSET_NAME = "QuestAsset";

        public Quest[] Quests
        {
            get { return quests; }
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

        /// <summary>Resets the progression by restoring all quests to their original default state</summary>
        public void Restore()
        {
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i].Restore();
            }
        }
    }
}