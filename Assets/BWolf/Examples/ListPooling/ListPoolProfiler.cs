using BWolf.Utilities.ListPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.ListPooling
{
    public class ListPoolProfiler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0), Tooltip("times the pooling happens")]
        private int excecuteTimes = 25;

        [SerializeField, Min(0), Tooltip("interval at which lists are created")]
        private float listCreateInterval = 0.5f;

        [SerializeField, Tooltip("ammount of lists created each excecution time")]
        private int listsCreatedEachInterval = 1000;

        [SerializeField, Tooltip("See difference between pooling and not pooling by setting this flag")]
        private bool usePooling = true;

        [Header("Profile")]
        [SerializeField, Tooltip("allocated bytes after lists are created. Set usePooling flag to see differrence")]
        private long allocatedBytes = 0L;

        private WaitForSeconds waitForSeconds;

        private void Awake()
        {
            waitForSeconds = new WaitForSeconds(listCreateInterval);
        }

        private void Start()
        {
            StartCoroutine(ListCreationEnumerator());
        }

        /// <summary>Enumerates ExcecuteTimes amount of times createing lists</summary>
        private IEnumerator ListCreationEnumerator()
        {
            for (int count = 0; count < excecuteTimes; count++)
            {
                CreateLists();
                yield return waitForSeconds;
            }
        }

        /// <summary>Creates lists either by pooling or using the new keyword and sets total memory allocated</summary>
        private void CreateLists()
        {
            long before = GC.GetTotalMemory(false);

            for (int count = 0; count < listsCreatedEachInterval; count++)
            {
                List<GameObject> gameObjects = usePooling ? ListPool<GameObject>.Create() : new List<GameObject>();
                foreach (GameObject go in gameObjects)
                {
                    //action
                }

                if (usePooling)
                {
                    ListPool<GameObject>.Dispose(gameObjects);
                }
            }

            for (int count = 0; count < listsCreatedEachInterval; count++)
            {
                List<Transform> gameObjects = usePooling ? ListPool<Transform>.Create() : new List<Transform>();
                foreach (Transform go in gameObjects)
                {
                    //action
                }

                if (usePooling)
                {
                    ListPool<Transform>.Dispose(gameObjects);
                }
            }

            long after = GC.GetTotalMemory(false);
            allocatedBytes = after - before;
        }
    }
}