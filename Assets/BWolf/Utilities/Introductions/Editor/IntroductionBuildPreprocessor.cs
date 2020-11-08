// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    /// <summary>Editor class for restoring progression assets before starting to build</summary>
    public class IntroductionBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            IntroductionAsset introAsset = Resources.Load<IntroductionAsset>(IntroductionAsset.ASSET_NAME);
            if (introAsset.RestoreOnBuild)
            {
                introAsset.Restore();
            }
        }
    }
}