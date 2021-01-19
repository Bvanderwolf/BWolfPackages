// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ProcessQueues
{
    /// <summary>
    /// A ScriptableObject holding a queue used for queueing processes
    /// </summary>
    /// <typeparam name="TProcessInfo"></typeparam>
    public class ProcessQueue<TProcessInfo> : ScriptableObject
    {
        private Queue<TProcessInfo> queue = new Queue<TProcessInfo>();

        /// <summary>
        /// Enqueues an object of type <typeparamref name="TProcessInfo"/>
        /// </summary>
        /// <param name="info"></param>
        public void Enqueue(TProcessInfo info)
        {
            queue.Enqueue(info);
        }

        /// <summary>
        /// Tries dequeing an object of type <typeparamref name="TProcessInfo"/>
        /// </summary>
        /// <param name="info"></param>
        public bool TryDequeue(out TProcessInfo info)
        {
            if (queue.Count != 0)
            {
                info = queue.Dequeue();
                return true;
            }
            else
            {
                info = default;
                return false;
            }
        }
    }
}