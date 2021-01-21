// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Utilities.CharacterDialogue;
using BWolf.Utilities.FileStorage;
using System;
using System.IO;
using UnityEngine;

namespace BWolf.Utilities.Introductions
{
    /// <summary>A scriptableobject representation of an introduction</summary>
    [CreateAssetMenu(menuName = "Introduction/Introduction")]
    public class Introduction : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private bool finished = false;

        [Space]
        [SerializeField, Tooltip("The introducables used for this introduction")]
        private Introducable[] introducables = null;

        [Header("Saving/Loading")]
        [SerializeField, Tooltip("The path relative to the root path of the app where introduction data will be stored. " +
           "Use the '.' as directory seperation character")]
        private string folderPath = "ProgressSaves.Introductions";

        [Header("References")]
        [SerializeField]
        private Monologue monologue = null;

        public event Action<Introduction, Monologue> OnStart;

        /// <summary>
        /// The cross platform path relative to the root path of the app where introduction data will be stored.
        /// </summary>
        public string FolderPath
        {
            get { return folderPath.Replace('.', Path.DirectorySeparatorChar); }
        }

        public bool Finished
        {
            get { return finished; }
        }

        /// <summary>Starts the introduction</summary>
        public void Start()
        {
            if (!finished)
            {
                OnIntroStart();
            }
        }

        /// <summary>Sets the finished state of this introduction</summary>
        public void SetFinished(bool value, bool saveToFile = true)
        {
            if (value == finished)
            {
                return;
            }

            finished = value;

            if (saveToFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Called when the introduction has started to bind the OnIntroContinue function</summary>
        private void OnIntroStart()
        {
            monologue.OnContinue += OnIntroContinue;
            OnStart(this, monologue);
        }

        /// <summary>Called when the intro is being continued at given index to update the stored introducables</summary>
        private void OnIntroContinue(int indexAt)
        {
            for (int i = 0; i < introducables.Length; i++)
            {
                introducables[i].Update(indexAt);
            }
        }

        public void Finish()
        {
            monologue.OnContinue -= OnIntroContinue;

            for (int i = 0; i < introducables.Length; i++)
            {
                introducables[i].End();
            }

            SetFinished(true);
        }

        /// <summary>Saves value to local storage</summary>
        private void SaveToFile()
        {
            string filePath = Path.Combine(FolderPath, name);
            FileStorageSystem.SaveAsBinary(filePath, finished);
        }

        /// <summary>Loads value from local storage</summary>
        public void LoadFromFile()
        {
            string filePath = Path.Combine(FolderPath, name);

            if (FileStorageSystem.LoadBinary(filePath, out bool finishedValue))
            {
                SetFinished(finishedValue, false);
            }
        }

        /// <summary>Restores this introduction to its default state</summary>
        [ContextMenu("Restore")]
        public void Restore()
        {
#if UNITY_EDITOR
            //make sure that in the editor, restoring the introduction outside of playmode doesn't cause any null references
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                OnStart = null;
            }

#endif
            SetFinished(false);

#if UNITY_EDITOR
            //make sure that in the editor, restoring the introduction it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>A sub structure for introduction to manage introducables in the scene</summary>
        [Serializable]
        private struct Introducable
        {
#pragma warning disable 0649

            [SerializeField, Tooltip("IntoTagName of Introducable Objects in scene")]
            private string tagName;

            [SerializeField, Tooltip("Dialogue index from which the introducable starts")]
            private int fromIndex;

            [SerializeField, Tooltip("Dialogue index at which the introducable stops")]
            private int ToIndex;

#pragma warning restore 0649

            private IntroducableObject introducableObject;

            /// <summary>Updates the introducable by either starting or ending the it based on given index</summary>
            public void Update(int indexAt)
            {
                if (indexAt == fromIndex)
                {
                    introducableObject = GetIntroducable();
                    introducableObject.StartIntroduction();
                }
                else if (indexAt == ToIndex)
                {
                    End();
                }
            }

            /// <summary>Ends the introducable</summary>
            public void End()
            {
                if (introducableObject != null)
                {
                    introducableObject.EndIntroduction();
                    introducableObject = null;
                }
            }

            /// <summary>Returns the introducable object behaviour in the scene based on stored tagname</summary>
            private IntroducableObject GetIntroducable()
            {
                foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag(IntroducableObject.COMPONENT_TAG_NAME))
                {
                    IntroducableObject introducable = gameObject.GetComponent<IntroducableObject>();
                    if (introducable.IntroTagName == tagName)
                    {
                        return introducable;
                    }
                }

                return null;
            }
        }
    }
}