using System;
using System.Collections;
using UnityEngine;

public static class PercExtensions 
{
    public class UpdateRoutine : IUpdateRoutine<Perc>
    {
        private MonoBehaviour _dispatcher;

        private Action<Perc> _updates;

        private Coroutine _routine;

        private float _percFrom;

        private float _percTo;

        private float _time;

        public UpdateRoutine(float from, float to, float time)
        {
            _percFrom = from;
            _percTo = to;
            _time = time;
        }

        public IUpdateRoutine<Perc> SetDispatcher(MonoBehaviour monoBehaviour)
        {
            _dispatcher = monoBehaviour;
            return this;
        }

        public IUpdateRoutine<Perc> OnUpdate(Action<Perc> action)
        {
            _updates += action;
            return this;
        }

        public void Start()
        {
            if (_dispatcher == null)
                throw new InvalidOperationException("Can't start coroutine :: no dispatcher set.");

            _routine = _dispatcher.StartCoroutine(Routine());
        }

        public void Stop()
        {
            if (_dispatcher == null)
                throw new InvalidOperationException("Can't stop coroutine :: no dispatcher set.");
            if (_routine == null)
                throw new InvalidOperationException("Can't stop coroutine :: no routine started.");
            
            _dispatcher.StopCoroutine(_routine);
            _routine = null;
        }

        private IEnumerator Routine()
        {
            float currentTime;
            Perc perc;

            currentTime = 0.0f;
            while (currentTime < _time)
            {
                perc = new Perc(Mathf.Lerp(_percFrom, _percTo, currentTime / _time));
                _updates?.Invoke(perc);
                
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    public static IUpdateRoutine<Perc> AddRoutine(this Perc perc, float value)
        => SetRoutine(perc, perc.value + value);

    public static IUpdateRoutine<Perc> RemoveRoutine(this Perc perc, float value)
        => SetRoutine(perc, perc.value - value);

    public static IUpdateRoutine<Perc> SetRoutine(this Perc perc, float value, float time = 1f)
        => new UpdateRoutine(perc.value, value, time);
}
