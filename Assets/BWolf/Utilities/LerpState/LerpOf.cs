// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.6
//----------------------------------

using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BWolf.Utilities
{
    /// <summary>
    /// Represents continuable linear interpolation operation data.
    /// </summary>
    /// <typeparam name="T">The type of object to do the interpolation between.</typeparam>
    [Serializable]
    public class LerpOf<T> : LerpState
    {
        /// <summary>
        /// The total time that needs to pass before the interpolation is finished.
        /// </summary>
        public override float TotalTime => _totalTime;

        /// <summary>
        /// The linearly interpolated value based on the current state. Will return the default value
        /// if no lerp function is set.
        /// </summary>
        public T Value => lerpFunction != null ? lerpFunction.Invoke(initial, target, Percentage) : default;

        /// <summary>
        /// The current percentage of linear interpolation.
        /// </summary>
        public override float Percentage => easingFunction(_currentTime / _totalTime);

        /// <summary>
        /// The remaining time to linearly interpolate.
        /// </summary>
        public override float RemainingTime => _totalTime - _currentTime;
        
        /// <summary>
        /// The initial value.
        /// </summary>
        [Header("State")]
        public T initial;
        
        /// <summary>
        /// The target value.
        /// </summary>
        public T target;
        
        /// <summary>
        /// The total time that needs to pass before the interpolation is finished.
        /// </summary>
        [SerializeField, Min(0)]
        private float _totalTime;

        /// <summary>
        /// The function used to return the current linearly interpolated value.
        /// Think of Vector3.Lerp for Vector3's and Quaterion.Lerp for Quaternions.
        /// </summary>
        public LerpFunction<T> lerpFunction;

        /// <summary>
        /// The way time is overriden when the total time is modified.
        /// </summary>
        [Header("Modification")]
        [Tooltip("The way time is overriden when the total time is modified.")]
        public TimeOverride timeOverride;

        /// <summary>
        /// The function used to ease the linear interpolation. 
        /// </summary>
        public EasingFunction easingFunction;
        
        /// <summary>
        /// Whether continuing the interpolation adds fixed delta time or normal delta time.
        /// </summary>
        [Tooltip("Whether continuing the interpolation adds fixed delta time or normal delta time.")]
        public bool usesFixedDelta;

        /// <summary>
        /// The current amount of time passed by continuing the interpolation. 
        /// </summary>
        private float _currentTime;

        /// <summary>
        /// Creates a new lerp instance.
        /// </summary>
        public LerpOf(
            T initial, 
            T target, 
            float totalTime = 1.0f, 
            EasingFunction easingFunction = null,
            LerpFunction<T> lerpFunction = null,
            TimeOverride timeOverride = TimeOverride.KEEP_CURRENT_TIME,
            bool usesFixedDelta = false)
        {
            this.initial = initial;
            this.target = target;
            this.easingFunction = easingFunction ?? EasingFunctions.noEase;
            this.lerpFunction = lerpFunction;
            this.timeOverride = timeOverride;
            this.usesFixedDelta = usesFixedDelta;
            
            _totalTime = totalTime;
        }
        
        /// <summary>
        /// Creates a new lerp instance using default values.
        /// </summary>
        protected LerpOf() : this(default, default)
        {
        }

        /// <summary>
        /// Continues the linear interpolation, updating the current state.
        /// </summary>
        /// <returns>Whether the interpolation has finished.</returns>
        public override bool Continue()
        {
            if (_currentTime >= _totalTime)
                return false;

            _currentTime += usesFixedDelta ? Time.fixedDeltaTime : Time.deltaTime;
            if (_currentTime > _totalTime)
                _currentTime = _totalTime;

            return true;
        }

        /// <summary>
        /// Sets the total time that needs to pass before the interpolation is finished.
        /// Uses the timeOverride member to determine what to do with the current time.
        /// </summary>
        /// <param name="newTotalTime">The new total time.</param>
        public void SetTotalTime(float newTotalTime)
        {
            switch (timeOverride)
            {
                case TimeOverride.KEEP_CURRENT_PERCENTAGE:
                    _currentTime = newTotalTime * Percentage;
                    break;
                
                case TimeOverride.RESET:
                    Reset();
                    break;
            }

            _totalTime = newTotalTime;
        }
        
        /// <summary>
        /// Resets the interpolation state, setting the current time to 0.
        /// </summary>
        public override void Reset() => _currentTime = 0.0f;
        
        /// <summary>
        /// Returns a routine that waits for this linear interpolation to finish.
        /// The routine will yield null, meaning the callback will invoke each frame.
        /// </summary>
        /// <param name="callback">The callback with the linearly interpolated value.</param>
        /// <returns>The routine.</returns>
        public IEnumerator Await(Action<T> callback) => Await(null, callback);

        /// <summary>
        /// Returns a routine that waits for this and another linear interpolation to finish.
        /// The routine will yield null, meaning the callback will invoke each frame.
        /// </summary>
        /// <param name="lerp">The additional linear interpolation to wait for.</param>
        /// <param name="callback">The callback with the linearly interpolated values.</param>
        /// <returns>The routine.</returns>
        public IEnumerator AwaitWith<T2>(LerpOf<T2> lerp, Action<T, T2> callback) => AwaitWith(lerp, null, callback);

        /// <summary>
        /// Returns a routine that waits for this linear interpolation to finish.
        /// </summary>
        /// <param name="instruction">The yield instruction to wait for (e.g. WaitForSeconds).</param>
        /// <param name="callback">The callback with the linearly interpolated value.</param>
        /// <returns>The routine.</returns>
        public IEnumerator Await(YieldInstruction instruction, Action<T> callback)
        {
            do
            {
                callback.Invoke(Value);
                yield return instruction;

            } while (Continue());
        }

        /// <summary>
        /// Returns a routine that waits for this and another linear interpolation to finish.
        /// </summary>
        /// <param name="lerp">The additional linear interpolation to wait for.</param>
        /// <param name="instruction">The yield instruction to wait for (e.g. WaitForSeconds).</param>
        /// <param name="callback">The callback with the linearly interpolated values.</param>
        /// <returns>The routine.</returns>
        public IEnumerator AwaitWith<T2>(LerpOf<T2> lerp, YieldInstruction instruction, Action<T, T2> callback)
        {
            do
            {
                callback.Invoke(Value, lerp.Value);
                yield return instruction;

            } while (Continue() && lerp.Continue());
        }

        /// <summary>
        /// Returns a routine that waits for given linear interpolations to finish.
        /// The routine will yield null, meaning the callback will invoke each frame.
        /// </summary>
        /// <param name="lerps">The linear interpolations to wait for.</param>
        /// <param name="callback">The callback with the linearly interpolated value.</param>
        /// <returns>The routine.</returns>
        public static IEnumerator Await(LerpOf<T>[] lerps, Action<T[]> callback) => Await(lerps, null, callback);

        /// <summary>
        /// Returns a routine that waits for given linear interpolations to finish.
        /// </summary>
        /// <param name="lerps">The linear interpolations to wait for.</param>
        /// <param name="instruction">The yield instruction to wait for (e.g. WaitForSeconds).</param>
        /// <param name="callback">The callback with the linearly interpolated value.</param>
        /// <returns>The routine.</returns>
        public static IEnumerator Await(LerpOf<T>[] lerps, YieldInstruction instruction, Action<T[]> callback)
        {
            T[] values = new T[lerps.Length];
            do
            {
                for (int i = 0; i < values.Length; i++)
                    values[i] = lerps[i].Value;

                callback.Invoke(values);
                yield return instruction;

            } while (lerps.Select(lerp => lerp.Continue()).All(continued => continued));
        }

        /// <summary>
        /// Returns a routine that wait for linear interpolations to finish given its arguments.
        /// </summary>
        /// <returns>The routine.</returns>
        public static IEnumerator Await(
            T initial,
            T target,
            LerpFunction<T> lerpFunction,
            Action<T> callback,
            float totalTime = 1.0f,
            EasingFunction easingFunction = null,
            bool usesFixedDelta = false)
        {
            return Await(initial, target, lerpFunction, null, callback, totalTime, easingFunction, usesFixedDelta);
        }

        /// <summary>
        /// Returns a routine that wait for linear interpolations to finish given its arguments.
        /// </summary>
        /// <returns>The routine.</returns>
        public static IEnumerator Await(
            T initial,
            T target,
            LerpFunction<T> lerpFunction,
            YieldInstruction instruction,
            Action<T> callback,
            float totalTime = 1.0f,
            EasingFunction easingFunction = null,
            bool usesFixedDelta = false)
        {

            return new LerpOf<T>(
                initial, 
                target, 
                totalTime, 
                easingFunction, 
                lerpFunction,
                TimeOverride.KEEP_CURRENT_TIME, 
                usesFixedDelta).Await(instruction, callback);
        }
    }
}