using BWolf.Utilities.PlayerProgression.PlayerProps;
using BWolf.Utilities.PlayerProgression.Quests;
using UnityEditor.Build;

// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression
{
    /// <summary>Editor class for restoring progression assets before starting to build</summary>
    public class ProgressionBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerPropertiesAsset playerPropertiesAsset = Resources.Load<PlayerPropertiesAsset>(PlayerPropertiesAsset.ASSET_NAME);
            if (playerPropertiesAsset.RestoreOnBuild)
            {
                playerPropertiesAsset.Restore();
            }

            QuestAsset questAsset = Resources.Load<QuestAsset>(QuestAsset.ASSET_NAME);
            if (questAsset.RestoreOnBuild)
            {
                questAsset.Restore();
            }
        }
    }
}