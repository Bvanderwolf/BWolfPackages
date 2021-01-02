using UnityEngine;

namespace BWolf.Utilities.SceneTransitioning
{
    /// <summary>
    /// Stores scene information to be edited in the inspector
    /// </summary>
    [CreateAssetMenu(fileName = "SceneInfo", menuName = "Scene Data/SceneInfo")]
    public partial class SceneInfoSO : ScriptableObject
    {
        [Header("General Scene Info")]
#if UNITY_EDITOR
        [SerializeField]
        private UnityEditor.SceneAsset sceneAsset = null;

#endif

        [Space]
        public string path = string.Empty;
    }
}