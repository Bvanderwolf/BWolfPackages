// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections;

namespace BWolf.Utilities.ProcessQueues
{
    /// <summary>
    /// Provides An abstract implementation of a processManager that broadcasts when a process has ended
    /// </summary>
    /// <typeparam name="TProcessInfo"></typeparam>
    public abstract class BroadcastingProcessManager<TProcessInfo> : ProcessManager<TProcessInfo>
    {
        protected abstract ProcessEventChannel<TProcessInfo> EventChannel { get; }

        protected abstract override ProcessQueue<TProcessInfo> ProcessQueue { get; }

        protected abstract override IEnumerator DoProcess(TProcessInfo info);

        protected override void OnCheckedForDeque(TProcessInfo oldInfo, TProcessInfo nextInfo, bool dequeued)
        {
            base.OnCheckedForDeque(oldInfo, nextInfo, dequeued);

            //broadcast the old process info to let listeners know that the process ended
            EventChannel.RaiseEvent(oldInfo);
        }
    }
}