using System;
using System.Collections;
using UnityEngine;

namespace BWolf.GameTasks
{
    public partial class GameTask : IEnumerator
    {
        public event Action Started;

        public event Action Ended;
        
        public bool HasEnded { get; private set; }
        
        public bool HasStarted { get; private set; }

        public bool IsActive => HasStarted && !HasEnded && !IsPaused;
        
        public bool IsPaused { get; private set; }
        
        private readonly MonoBehaviour _behaviour;

        private readonly IEnumerator _routine;

        public GameTask(MonoBehaviour behaviour, IEnumerator routine)
        {
            _behaviour = behaviour;
            _routine = routine;
        }

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

        public void Continue() => IsPaused = false;

        public void Pause() => IsPaused = true;

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

        public void Reset()
        {
            if(IsActive)
                _behaviour.StopCoroutine(this);
            
            HasStarted = false;
            HasEnded = false;
        }

        public object Current => (object)null;
    }
}


