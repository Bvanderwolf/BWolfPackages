// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents continuable ping-pong operation data.
    /// </summary>
    [Serializable]
    public class PingPong : IContinuable, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The current ping pong value.
        /// </summary>
        public float Value => min + Mathf.PingPong(_currentTime, MinMaxDifference);

        /// <summary>
        /// The percentage of completion.
        /// </summary>
        public float Percentage => (_currentTime - StartTime) / _totalTime;

        /// <summary>
        /// The remaining time to operate.
        /// </summary>
        public float RemainingTime => _totalTime - _currentTime;
        
        /// <summary>
        /// The difference between the min and max values.
        /// </summary>
        public float MinMaxDifference => Mathf.Abs(min - max);

        /// <summary>
        /// The starting time based on the starting percentage.
        /// </summary>
        public float StartTime => startPercentage * MinMaxDifference;

        /// <summary>
        /// The total time necessary for the operation without accounting for speed.
        /// </summary>
        public float TotalTime => _totalTime;

        /// <summary>
        /// The minimum value.
        /// </summary>
        public float min;
        
        /// <summary>
        /// The maximum value.
        /// </summary>
        public float max;

        /// <summary>
        /// The speed of the operation.
        /// </summary>
        public float speed = 1.0f;

        /// <summary>
        /// The starting percentage at which to start the operation.
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float startPercentage;

        /// <summary>
        /// Whether continuing the operation adds fixed delta time or normal delta time.
        /// </summary>
        public bool usesFixedDelta;
        
        /// <summary>
        /// The amount of times to ping pong.
        /// </summary>
        public int count;

        /// <summary>
        /// The total time necessary for the operation without accounting for speed.
        /// </summary>
        private float _totalTime;
        
        /// <summary>
        /// The current time in of the operation.
        /// </summary>
        private float _currentTime;

        /// <summary>
        /// Creates a new ping pong instance.
        /// </summary>
        public PingPong(float min, float max, int count, float startPercentage) 
            : this(min, max, count, 1.0f, startPercentage)
        {
        }

        /// <summary>
        /// Creates a new ping pong instance.
        /// </summary>
        public PingPong(
            float min, 
            float max, 
            int count,
            float speed = 1.0f,
            float startPercentage = 0.0f, 
            bool usesFixedDelta = false)
        {
            if (min > max)
                throw new ArgumentException($"min: {min} is greater than max: {max} :: this is not intended!");

            this.min = min;
            this.max = max;
            this.usesFixedDelta = usesFixedDelta;
            this.count = count;
            this.speed = speed;

            OnAfterDeserialize();
        }

        /// <summary>
        /// Continues the ping pong operation, updating the current state.
        /// </summary>
        /// <returns>Whether the operation has finished.</returns>
        public bool Continue()
        {
            if (_currentTime >= _totalTime)
                return false;

            _currentTime += (usesFixedDelta ? Time.fixedDeltaTime : Time.deltaTime) * speed;
            if (_currentTime > _totalTime)
                _currentTime = _totalTime;
            
            return true;
        }

        /// <summary>
        /// Called by unity before serializing this object.
        /// </summary>
        public void OnBeforeSerialize() { }

        /// <summary>
        /// Called by unity after serializing this object, updating its state.
        /// </summary>
        public void OnAfterDeserialize()
        {
            _currentTime = StartTime;
            _totalTime = ((count * 2f) * MinMaxDifference) + StartTime;
        }
    }
}