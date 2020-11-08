using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    /// <summary>An introduction used to introduce a scene</summary>
    [CreateAssetMenu(menuName = "Introduction/SceneIntroduction")]
    public class SceneIntroduction : Introduction
    {
        [Header("SceneSettings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        public string NameOfScene
        {
            get { return nameOfScene; }
        }
    }
}