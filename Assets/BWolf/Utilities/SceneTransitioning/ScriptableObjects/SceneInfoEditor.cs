// Created By: Unity Open Project @ https://github.com/UnityTechnologies/open-project-1
//----------------------------------

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// The Editor part of scene info that updates the scene path based on the stored scene asset
    /// </summary>
    public partial class SceneInfoSO : ScriptableObject, ISerializationCallbackReceiver
    {
        private SceneAsset prevSceneAsset;

        private void OnEnable()
        {
            //reset scene path on domain reload
            prevSceneAsset = null;
            UpdateScenePath();
        }

        public void OnBeforeSerialize()
        {
            //check for updates of scene asset in inspector
            UpdateScenePath();
        }

        private void UpdateScenePath()
        {
            if (sceneAsset != null)
            {
                if (prevSceneAsset != sceneAsset)
                {
                    //only update path when the scene asset has a new value
                    prevSceneAsset = sceneAsset;
                    path = AssetDatabase.GetAssetOrScenePath(sceneAsset);
                }
            }
            else
            {
                //if there is not scene asset scene the path is an empty string
                path = string.Empty;
            }
        }

        public void OnAfterDeserialize()
        {
        }
    }
}

#endif