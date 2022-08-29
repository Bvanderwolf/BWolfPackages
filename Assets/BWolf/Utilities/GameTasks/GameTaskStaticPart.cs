using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.GameTasks
{
    public partial class GameTask
    {
        private struct Aggregate
        {
            public MonoBehaviour behaviour;

            public GameTask[] tasks;

            public Aggregate(MonoBehaviour behaviour, GameTask task)
            {
                this.behaviour = behaviour;
                tasks = new GameTask[] { task };
            }

            public Aggregate Add(GameTask task)
            {
                tasks.Increment(task);
                return this;
            }

            public Aggregate Remove(GameTask task)
            {
                int index = Array.FindIndex(tasks, t => t == task);
                tasks.RemoveAt(index);
                return this;
            }
        }
        
        private static readonly List<Aggregate> _aggregates = new List<Aggregate>();

        public static int Count()
        {
            int count = 0;

            for (int i = 0; i < _aggregates.Count; i++)
                count += _aggregates[i].tasks.Length;

            return count;
        }

        public static int Count(MonoBehaviour behaviour)
        {
            for (int i = 0; i < _aggregates.Count; i++)
                if (_aggregates[i].behaviour == behaviour)
                    return _aggregates[i].tasks.Length;

            return 0;
        }

        public static int Count(GameObject gameObject)
        {
            int count = 0;
            
            for (int i = 0; i < _aggregates.Count; i++)
                if (_aggregates[i].behaviour.gameObject == gameObject)
                    count += _aggregates[i].tasks.Length;

            return count;
        }

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

        public static GameTask Run(MonoBehaviour behaviour, IEnumerator routine, Action endedCallback = null)
        {
            GameTask task = new GameTask(behaviour, routine);
            if (endedCallback != null)
                task.Ended += endedCallback;
            
            task.Start();
            return task;
        }

        private static void AddActiveTask(MonoBehaviour behaviour, GameTask task)
        {
            int index = _aggregates.FindIndex(agr => agr.behaviour == behaviour);
            if (index == -1)
                _aggregates.Add(new Aggregate(behaviour, task));
            else
                _aggregates[index] = _aggregates[index].Add(task);
        }

        private static void RemoveActiveTask(MonoBehaviour behaviour, GameTask task)
        {
            Aggregate aggregate;
            
            for (int i = _aggregates.Count - 1; i >= 0; i--)
            {
                aggregate = _aggregates[i];
                if (aggregate.behaviour != behaviour)
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
