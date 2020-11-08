// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using BWolf.Behaviours.SingletonBehaviours;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BWolf.Utilities.Introductions
{
    /// <summary>Singleton class for managing introductions</summary>
    public class IntroductionManager : SingletonBehaviour<IntroductionManager>
    {
        [Header("References")]
        [SerializeField]
        private IntroductionAsset asset = null;

        [SerializeField]
        private GameObject prefabIntroArrow = null;

        public event Action IntroFinished;

        public bool IsActive { get; private set; }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            string nameOfActiveScene = SceneManager.GetActiveScene().name;
            foreach (Introduction introduction in asset)
            {
                introduction.OnFinish += OnIntroFinished;
                introduction.OnStart += OnIntroStart;
                introduction.LoadFromFile();

                SceneIntroduction sceneIntroduction = introduction as SceneIntroduction;
                if (sceneIntroduction != null && !sceneIntroduction.Finished && sceneIntroduction.NameOfScene == nameOfActiveScene)
                {
                    sceneIntroduction.Start();
                }
            }
        }

        protected override void OnDestroy()
        {
            foreach (Introduction introduction in asset)
            {
                introduction.OnFinish -= OnIntroFinished;
            }
        }

        /// <summary>Returns a new IntroductionArrow gameobject</summary>
        public GameObject GetArrow(Transform parent = null)
        {
            return Instantiate(prefabIntroArrow, parent ?? transform);
        }

        /// <summary>Sets the active state of the introduction manager when an introduction has started</summary>
        private void OnIntroStart()
        {
            if (!IsActive)
            {
                IsActive = true;
            }
            else
            {
                Debug.LogError("An intro has started while another was already in progress :: this is not intended behaviour!");
            }
        }

        /// <summary>Called when a introduction has been finished to fire event and set active state</summary>
        private void OnIntroFinished()
        {
            IsActive = false;
            IntroFinished?.Invoke();
        }

        /// <summary>Restores the introductions to their default state</summary>
        [ContextMenu("RestoreIntroductions")]
        public void RestoreIntroductions()
        {
            asset.Restore();
        }

        /// <summary>Disables introductions by setting them all to a finished state</summary>
        [ContextMenu("DisableIntroduction")]
        public void DisableIntroduction()
        {
            asset.SetAllFinished();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //start scene intro if there is one available
            SceneIntroduction intro = asset.GetSceneIntroduction(scene.name);
            if (intro != null)
            {
                intro.Start();
            }
        }
    }
}