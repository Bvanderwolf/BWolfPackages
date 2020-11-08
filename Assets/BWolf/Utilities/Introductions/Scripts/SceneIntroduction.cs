// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    /// <summary>An introduction used to introduce a scene</summary>
    [CreateAssetMenu(menuName = "Introduction/SceneIntroduction")]
    public class SceneIntroduction : Introduction
    {
        [Header("SceneSettings")]
        [SerializeField, Tooltip("The name of the scene on which this introduction is given on load")]
        private string nameOfScene = string.Empty;

        public string NameOfScene
        {
            get { return nameOfScene; }
        }
    }
}