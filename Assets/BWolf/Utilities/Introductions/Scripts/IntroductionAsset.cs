// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;
using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    /// <summary>Container asset for storing introductions</summary>
    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "Introduction/Asset")]
    public class IntroductionAsset : ScriptableObject, IEnumerable
    {
        [Header("Settings")]
        [SerializeField]
        private bool restoreOnBuild = true;

        [Header("Introductions")]
        [SerializeField]
        private Introduction[] introductions = null;

        public const string ASSET_NAME = "IntroductionAsset";

        public bool RestoreOnBuild
        {
            get { return restoreOnBuild; }
        }

        /// <summary>loads the finished state for each introduction</summary>
        public void LoadFromFile()
        {
            for (int i = 0; i < introductions.Length; i++)
            {
                introductions[i].LoadFromFile();
            }
        }

        /// <summary>Sets all stored introductions to be finished</summary>
        public void SetAllFinished()
        {
            for (int i = 0; i < introductions.Length; i++)
            {
                introductions[i].SetFinished(true);
            }
        }

        /// <summary>Restores all stored introductions to their default state</summary>
        public void Restore()
        {
            for (int i = 0; i < introductions.Length; i++)
            {
                introductions[i].Restore();
            }
        }

        /// <summary>Returns a scene introduction object with given name</summary>
        public SceneIntroduction GetSceneIntroduction(string nameOfScene)
        {
            for (int i = 0; i < introductions.Length; i++)
            {
                SceneIntroduction intro = introductions[i] as SceneIntroduction;
                if (intro != null && !intro.Finished && intro.NameOfScene == nameOfScene)
                {
                    return intro;
                }
            }

            return null;
        }

        /// <summary>Returns the enumerator for the introductions array</summary>
        public IEnumerator GetEnumerator()
        {
            return introductions.GetEnumerator();
        }
    }
}