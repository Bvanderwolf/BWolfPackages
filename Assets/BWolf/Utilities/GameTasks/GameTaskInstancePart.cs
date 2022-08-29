using System;
using System.Collections;
using UnityEngine;

namespace BWolf.GameTasks
{
    /// <summary>
    /// Represents a game task to be executed as a coroutine on a mono behaviour.
    /// </summary>
    public partial class GameTask : IEnumerator
    {
        /// <summary>
        /// Fired when the task has started.
        /// </summary>
        public event Action Started;

        /// <summary>
        /// Fired when the task has ended.
        /// </summary>
        public event Action Ended;
        
        /// <summary>
        /// Whether the task has ended.
        /// </summary>
        public bool HasEnded { get; private set; }
        
        /// <summary>
        /// Whether the task has started.
        /// </summary>
        public bool HasStarted { get; private set; }

        /// <summary>
        /// Whether the task is currently active.
        /// </summary>
        public bool IsActive => HasStarted && !HasEnded && !IsPaused;
        
        /// <summary>
        /// Whether the task is currently paused.
        /// </summary>
        public bool IsPaused { get; private set; }
        
        /// <summary>
        /// The behaviour used to run the task coroutine.
        /// </summary>
        private readonly MonoBehaviour _behaviour;

        /// <summary>
        /// The pointer towards the task routine to be run.
        /// </summary>
        private readonly IEnumerator _routine;

        /// <summary>
        /// Creates a new instance of the game task using a behaviour to run the task coroutine
        /// and the pointer towards the task routine to be run.
        /// </summary>
        /// <param name="behaviour">The behaviour used to run the task coroutine.</param>
        /// <param name="routine">The pointer towards the task routine to be run.</param>
        public GameTask(MonoBehaviour behaviour, IEnumerator routine)
        {
            _behaviour = behaviour;
            _routine = routine;
        }

        /// <summary>
        /// Starts the task.
        /// </summary>
        public void Start()
        {
            if (HasStarted)
            {
                Debug.LogWarning("Can't start routine :: Routine has already started");
                return;
            }
            
            _behaviour.StartCoroutine(this);
            AddActiveTask(_behaviour, this);
        }

        /// <summary>
        /// Continues the task if it is paused.
        /// </summary>
        public void Continue()
        {
            if (!IsPaused)
                return;
            
            _behaviour.StartCoroutine(this);
            IsPaused = false;
        }

        /// <summary>
        /// Pauses the task if it is active.
        /// </summary>
        public void Pause()
        {
            if (!IsActive)
                return;
            
            _behaviour.StopCoroutine(_routine);
            IsPaused = true;
        }

        /// <summary>
        /// IEnumerator method to increment the routine state. Returns whether it succeeded.
        /// *This is to be used by unity internally. Do not use manually.*
        /// </summary>
        /// <returns>Whether the routine state could be incremented.</returns>
        public bool MoveNext()
        {
            if (IsPaused)
                return true;
            
            if (!HasStarted)
            {
                HasStarted = true;
                Started?.Invoke();
            }
            
            bool movedNext = _routine.MoveNext();
            if (movedNext)
                return true;
            
            HasEnded = true;
            RemoveActiveTask(_behaviour, this);
            Ended?.Invoke();
            return false;
        }
        
        public void Reset() { }

        public object Current => (object)null;
    }
}


