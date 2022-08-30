using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.GameTasks
{
    /// <summary>
    /// Represents a game task to be executed as a coroutine on a mono behaviour.
    /// </summary>
    public partial class GameTask
    {
        /// <summary>
        /// Stores a reference to a mono behaviour with the tasks its running.
        /// Used to store and provide static state info.
        /// </summary>
        private readonly struct Aggregate
        {
            /// <summary>
            /// The behaviour running the game tasks.
            /// </summary>
            public readonly MonoBehaviour behaviour;

            /// <summary>
            /// The game tasks run by the behaviour.
            /// </summary>
            public readonly GameTask[] tasks;

            /// <summary>
            /// Creates a new aggregate with a behaviour running the game tasks
            /// and a game task the behaviour starts with.
            /// </summary>
            /// <param name="behaviour">The behaviour.</param>
            /// <param name="task">The first game task run by the behaviour.</param>
            public Aggregate(MonoBehaviour behaviour, GameTask task)
            {
                this.behaviour = behaviour;
                tasks = new GameTask[] { task };
            }

            /// <summary>
            /// Adds a new game task to the aggregate.
            /// </summary>
            /// <param name="task">The task to be stored.</param>
            /// <returns>The aggregate.</returns>
            public Aggregate Add(GameTask task)
            {
                tasks.Increment(task);
                return this;
            }

            /// <summary>
            /// Removes a new game task from the aggregate.
            /// </summary>
            /// <param name="task">The task to be removed.</param>
            /// <returns>The aggregate.</returns>
            public Aggregate Remove(GameTask task)
            {
                int index = Array.FindIndex(tasks, t => t == task);
                tasks.RemoveAt(index);
                return this;
            }
        }
        
        /// <summary>
        /// The static list of aggregates holding game tasks run by mono behaviours.
        /// </summary>
        private static readonly List<Aggregate> _aggregates = new List<Aggregate>();

        /// <summary>
        /// An enumerable for selecting the amount of tasks from the list of aggregates.
        /// </summary>
        private static readonly IEnumerable<int> _lengthSelector 
            = _aggregates.Select(agr => agr.tasks.Length);
        
        /// <summary>
        /// An enumerable for selecting the amount of active tasks from the list of aggregates.
        /// </summary>
        private static readonly IEnumerable<int> _activeLengthSelector 
            = _aggregates.Select(agr => agr.tasks.Count(t => t.IsActive));

        /// <summary>
        /// Returns the amount of tasks currently being run.
        /// </summary>
        /// <param name="includePaused">Whether to include paused tasks.</param>
        /// <returns>The amount of tasks currently being run.</returns>
        public static int Count(bool includePaused = false)
        {
            if (_aggregates.Count == 0)
                return 0;

            IEnumerable<int> lengths = includePaused ? _lengthSelector : _activeLengthSelector;
            return lengths.Aggregate((length, total) => total + length);
        }

        /// <summary>
        /// Returns the amount of tasks currently being run by a given behaviour.
        /// </summary>
        /// <param name="behaviour">The behaviour running the tasks.</param>
        /// <param name="includePaused">Whether to include paused tasks.</param>
        /// <returns>The amount of tasks being run.</returns>
        public static int Count(MonoBehaviour behaviour, bool includePaused = false)
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];

                if (aggregate.behaviour == behaviour)
                    return includePaused ? aggregate.tasks.Length : aggregate.tasks.Count(t => t.IsActive);
            }

            return 0;
        }

        /// <summary>
        /// Returns the amount of tasks currently being run by behaviours on a given game object..
        /// </summary>
        /// <param name="gameObject">The game object with behaviours running the tasks.</param>
        /// <param name="includePaused">Whether to include paused tasks.</param>
        /// <returns>The amount of tasks being run.</returns>
        public static int Count(GameObject gameObject, bool includePaused = false)
        {
            Aggregate aggregate;
            int count = 0;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];

                if (aggregate.behaviour.gameObject == gameObject)
                    count += includePaused ? aggregate.tasks.Length : aggregate.tasks.Count(t => t.IsActive);
            }

            return count;
        }

        /// <summary>
        /// Returns whether there are any active tasks on the given mono behaviour.
        /// </summary>
        /// <param name="behaviour">The behaviour to check.</param>
        /// <returns>Whether there are any active tasks on it.</returns>
        public static bool AnyActive(MonoBehaviour behaviour)
        {
            Aggregate aggregate;
            
            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour != behaviour)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    if (aggregate.tasks[j].IsActive)
                        return true;
                
                return false;
            }

            return false;
        }
        
        /// <summary>
        /// Returns whether there are any active tasks on the given game object.
        /// </summary>
        /// <param name="gameObject">The game object to check.</param>
        /// <returns>Whether there are any active tasks on it.</returns>
        public static bool AnyActive(GameObject gameObject)
        {
            Aggregate aggregate;
            
            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour.gameObject != gameObject)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    if (aggregate.tasks[j].IsActive)
                        return true;
            }

            return false;
        }
        
        /// <summary>
        /// Returns whether there are any active tasks using a user defined
        /// condition. Use this when you want to check for tasks running on, for example,
        /// game objects with specific tags.
        /// </summary>
        /// <param name="predicate">The user defined predicate, checking a game object.</param>
        /// <returns>Whether there are any active tasks using the predicate.</returns>
        public static bool AnyActive(Func<GameObject, bool> predicate)
        {
            Aggregate aggregate;
            
            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (!predicate.Invoke(aggregate.behaviour.gameObject))
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    if (aggregate.tasks[j].IsActive)
                        return true;
            }

            return false;
        }
        
        /// <summary>
        /// Returns whether there are any active tasks running.
        /// </summary>
        /// <returns>Whether there are any active tasks running.</returns>
        public static bool AnyActive()
        {
            Aggregate aggregate;
            
            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    if (aggregate.tasks[j].IsActive)
                        return true;
            }

            return false;
        }

        /// <summary>
        /// Pauses all active tasks on a given mono behaviour.
        /// </summary>
        /// <param name="behaviour">The behaviour to pause the tasks on.</param>
        public static void PauseAll(MonoBehaviour behaviour)
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour != behaviour)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Pause();

                break;
            }
        }
        
        /// <summary>
        /// Pauses all active tasks on a game object.
        /// </summary>
        /// <param name="gameObject">The game object to pause the tasks on.</param>
        public static void PauseAll(GameObject gameObject)
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour.gameObject != gameObject)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Pause();
            }
        }
        
        /// <summary>
        /// Pauses all active tasks.
        /// </summary>
        public static void PauseAll()
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Pause();
            }
        }
        
        /// <summary>
        /// Continues all paused tasks on a given mono behaviour.
        /// </summary>
        /// <param name="behaviour">The behaviour to continue the tasks on.</param>
        public static void ContinueAll(MonoBehaviour behaviour)
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour != behaviour)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Continue();

                break;
            }
        }
        
        /// <summary>
        /// Continues all paused tasks on a given game object.
        /// </summary>
        /// <param name="gameObject">The game object to continue the tasks on.</param>
        public static void ContinueAll(GameObject gameObject)
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour.gameObject != gameObject)
                    continue;
                
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Continue();
            }
        }
        
        /// <summary>
        /// Continues all paused tasks.
        /// </summary>
        public static void ContinueAll()
        {
            Aggregate aggregate;

            for (int i = 0; i < _aggregates.Count; i++)
            {
                aggregate = _aggregates[i];
                for (int j = 0; j < aggregate.tasks.Length; j++)
                    aggregate.tasks[j].Continue();
            }
        }

        /// <summary>
        /// Creates a new game task, starts it and returns the reference to it.
        /// </summary>
        /// <param name="behaviour">The mono behaviour to run the task on.</param>
        /// <param name="routine">The routine to run.</param>
        /// <param name="endedCallback">The callback for when the task has ended.</param>
        /// <returns>The reference to the started task.</returns>
        public static GameTask Run(MonoBehaviour behaviour, IEnumerator routine, Action endedCallback = null)
        {
            GameTask task = new GameTask(behaviour, routine);
            if (endedCallback != null)
                task.Ended += endedCallback;
            
            task.Start();
            return task;
        }

        /// <summary>
        /// Adds a started task to the static store to make it available for global query.
        /// </summary>
        /// <param name="task">The game task that has started.</param>
        private static void StoreStartedTask(GameTask task)
        {
            int index = _aggregates.FindIndex(agr => agr.behaviour == task._behaviour);
            if (index == -1)
                _aggregates.Add(new Aggregate(task._behaviour, task));
            else
                _aggregates[index] = _aggregates[index].Add(task);
        }

        /// <summary>
        /// Remove a task that has ended from the static store.
        /// </summary>
        /// <param name="task">The task that has ended.</param>
        private static void RemoveEndedTask(GameTask task)
        {
            Aggregate aggregate;
            
            for (int i = _aggregates.Count - 1; i >= 0; i--)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour != task._behaviour)
                    continue;

                if (aggregate.tasks.Length == 1 && aggregate.tasks[0] == task)
                {
                    _aggregates.RemoveAt(i);
                    break;
                }

                _aggregates[i] = _aggregates[i].Remove(task);
            }
        }
    }
}
