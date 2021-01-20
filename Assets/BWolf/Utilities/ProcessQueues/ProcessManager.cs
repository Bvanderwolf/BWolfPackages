// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;
using System.Collections;

namespace BWolf.Utilities.ProcessQueues
{
    /// <summary>
    /// Provides the necessary functionalities for managing processes using queues
    /// </summary>
    /// <typeparam name="TProcessInfo"></typeparam>
    public abstract class ProcessManager<TProcessInfo> : MonoBehaviour
    {
        /// <summary>
        /// The property used for managing the queue. Implement this by returning a reference to
        /// your own implementation of a progression queue of type <typeparamref name="TProcessInfo"/>
        /// </summary>
        protected abstract ProcessQueue<TProcessInfo> ProcessQueue { get; }

        protected bool isProcessing;

        public void StartProcess(TProcessInfo info)
        {
            if (!isProcessing)
            {
                //start managing a new process if there is not process active
                StartCoroutine(ManageProcess(info));
            }
            else
            {
                //queue the info for starting a process later
                ProcessQueue.Enqueue(info);
            }
        }

        private IEnumerator ManageProcess(TProcessInfo info)
        {
            OnProcessStart(info);
            yield return DoProcess(info);
            CheckForDeque(info);
        }

        /// <summary>
        /// Checks whether a new process can be started based on whether there is info left int he queue
        /// </summary>
        /// <param name="finishedProcessInfo"></param>
        private void CheckForDeque(TProcessInfo finishedProcessInfo)
        {
            bool dequeued = ProcessQueue.TryDequeue(out TProcessInfo nextProcessInfo);
            if (dequeued)
            {
                StartCoroutine(ManageProcess(nextProcessInfo));
            }
            else
            {
                isProcessing = false;
            }

            OnCheckedForDeque(finishedProcessInfo, nextProcessInfo, dequeued);
        }

        /// <summary>
        /// Checks the process queue if a process can be started and starts it based on the dequed information
        /// if succesfull
        /// </summary>
        protected void CheckForDeque()
        {
            if (ProcessQueue.TryDequeue(out TProcessInfo nextProcessInfo))
            {
                StartCoroutine(ManageProcess(nextProcessInfo));
            }
        }

        protected abstract IEnumerator DoProcess(TProcessInfo info);

        /// <summary>
        /// Called after the check for deque has been done
        /// </summary>
        /// <param name="oldInfo"></param>
        /// <param name="nextInfo"></param>
        /// <param name="dequeued"></param>
        protected virtual void OnCheckedForDeque(TProcessInfo oldInfo, TProcessInfo nextInfo, bool dequeued)
        {
        }

        /// <summary>
        /// Called before the DoProcess Routine has been started. Make sure the base of this function is called
        /// </summary>
        /// <param name="info"></param>
        protected virtual void OnProcessStart(TProcessInfo info)
        {
            isProcessing = true;
        }
    }
}