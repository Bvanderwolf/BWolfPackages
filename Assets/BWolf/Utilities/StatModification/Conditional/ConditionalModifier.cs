// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace BWolf.Utilities.StatModification
{
    /// <summary>
    /// Represents a point statistic modifier that keeps modifying until
    /// a given condition has been met.
    /// </summary>
    public class ConditionalModifier : PointStatModifier
    {
        /// <summary>
        /// The default condition used when no condition is given, running
        /// the modification until its being removed manually.
        /// </summary>
        private static readonly Func<bool> _defaultCondition = () => false;
        
        /// <summary>
        /// The interval at which the modification will be done.
        /// </summary>
        private float _interval = 1f;
        
        /// <summary>
        /// The amount of value this modifier will modify each interval while active
        /// </summary>
        private int _valuePerInterval = 0;

        /// <summary>
        /// The time passed to be incremented by delta time.
        /// </summary>
        private float _timePassed;

        /// <summary>
        /// The currently accumulated value.
        /// </summary>
        private int _currentValue;
        
        /// <summary>
        /// The value modified.
        /// </summary>
        private int _valueModified;

        /// <summary>
        /// The second passed event.
        /// </summary>
        private SecondPassedEvent _secondPassed;
        
        /// <summary>
        /// The stop condition for this modifier.
        /// </summary>
        private Func<bool> _stopCondition;

        /// <summary>
        /// Whether the stop condition has been met.
        /// </summary>
        public override bool Finished => _stopCondition();

        /// <summary>
        /// Creates a new instance of this stat modifier using a conditional modifier info object.
        /// </summary>
        public ConditionalModifier(ConditionInfo info)
        {
            Name = info.name;
            IncreasesValue = info.increasesValue;
            ModifiesCurrent = info.modifiesCurrent;
            ModifiesCurrentWithMax = info.modifiesCurrentWithMax;
            IsStackable = info.canStack;

            _stopCondition = info.StopCondition ?? _defaultCondition;
            _secondPassed = info.OnSecondsPassed;
            
            _valuePerInterval = info.value;
            _interval = info.interval;
        }

        /// <summary>Sets given condition as to when to stop this modifier</summary>
        public ConditionalModifier ModifyUntil(Func<bool> stopCondition)
        {
            _stopCondition = stopCondition;
            return this;
        }

        /// <summary>Executes function each second the system has been modified, providing the name of this modifier as a string and the value modified as an integer</summary>
        public ConditionalModifier OnSecondPassed(UnityAction<string, int> callback)
        {
            _secondPassed.AddListener(callback);
            return this;
        }

        /// <summary>Modifies system by regenerating or decaying given value, resseting and calling on second passed each second</summary>
        public override void Modify(PointStatSystem system)
        {
            _timePassed += Time.deltaTime;
            _currentValue = (int)(_timePassed * _valuePerInterval);
            if (_currentValue != _valueModified)
            {
                int difference = Mathf.Abs(_currentValue - _valueModified);
                _valueModified += difference;
                if (ModifiesCurrent)
                {
                    system.ModifyCurrent(this, IncreasesValue ? difference : -difference);
                }
                else
                {
                    system.ModifyMax(this, IncreasesValue ? difference : -difference);
                    if (ModifiesCurrentWithMax && !system.IsFull)
                    {
                        system.ModifyCurrent(this, IncreasesValue ? difference : -difference);
                    }
                }
            }
            if (_timePassed >= _interval)
            {
                _secondPassed?.Invoke(Name, _valueModified);
                _timePassed = 0;
                _valueModified = 0;
            }
        }

        /// <summary>Returns the string representation of this conditional stat modifier</summary>
        public override string ToString()
        {
            return $"ConditionalStatModifier[name: {Name}, valuePerSecond: {_valuePerInterval}, increase: {IncreasesValue}, modifiesCurrent: {ModifiesCurrent}, canStack: {IsStackable}]";
        }
    }
}