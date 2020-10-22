using BWolf.Utilities.PlayerProgression.PlayerProps;
using BWolf.Utilities.PlayerProgression.Quests;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>Editor class for restoring progression assets before starting to build</summary>
    public class ProgressionBuildPreprocessor : IPreprocessBuildWithReport
    {
        private const string PLAYER_PROPERTIES_ASSET_NAME = "PlayerPropertiesAsset";
        private const string QUEST_ASSET_NAME = "QuestAsset";

        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerPropertiesAsset playerPropertiesAsset = Resources.Load<PlayerPropertiesAsset>(PLAYER_PROPERTIES_ASSET_NAME);
            if (playerPropertiesAsset.RestoreOnBuild)
            {
                playerPropertiesAsset.Restore();
            }

            QuestAsset questAsset = Resources.Load<QuestAsset>(QUEST_ASSET_NAME);
            if (questAsset.RestoreOnBuild)
            {
                questAsset.Restore();
            }
        }
    }
}