using UnityEngine;

namespace BWolf.SceneSearch
{
    [CreateAssetMenu(menuName = "ScenePath/Config", fileName = nameof(ScenePathConfig))]
    public class ScenePathConfig : ScriptableObject
    {
        [SerializeField]
        private string[] _paths;

        public string[] GetGameObjectPaths()
        {
            string[] paths = new string[_paths.Length];
            for (int i = 0; i < paths.Length; i++)
                paths[i] = _paths[i];
            return paths;
        }
    }
}