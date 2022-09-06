using System;
using UnityEngine;

namespace BWolf.PointModifications
{
    public class ConditionalPointModifier : PointModifier
    {
        public float interval;
        
        private readonly Func<bool> _condition;
        
        private float _intervalPassed;

        private int _valueModified;

        private int _currentValue;

        public override bool IsFinished => _condition.Invoke();

        public ConditionalPointModifier(string name, int value, Func<bool> condition, float interval = 0f) : base(name, value)
        {
            _condition = condition;
            this.interval = interval;
        }
        
        public ConditionalPointModifier(string name, int value, Func<bool> condition, bool modifiesCurrent, float interval = 0f) : base(name, value, modifiesCurrent)
        {
            _condition = condition;
            this.interval = interval;
        }
        
        public ConditionalPointModifier(
            string name, 
            int value, 
            Func<bool> condition, 
            bool modifiesCurrent, 
            float interval = 0f,
            bool modifiesCurrentWithMax = false) 
            : base(name, value, modifiesCurrent, modifiesCurrentWithMax)
        {
            _condition = condition;
            this.interval = interval;
        }
        
        public override PointModification Modify(PointSystem pointSystem)
        {
            _intervalPassed += Time.deltaTime;
            _currentValue = (int)(_intervalPassed * value);
            
            if (_currentValue == _valueModified)
                return PointModification.None;
            
            int difference = Mathf.Abs(_currentValue - _valueModified);
            if (_intervalPassed >= interval)
            {
                _intervalPassed = 0;
                _valueModified = 0;
            }
            else
            {
                _valueModified += difference;
            }

            return new PointModification
            {
                value = difference,
                modifiesCurrent = modifiesCurrent,
                modifiesCurrentWithMax = modifiesCurrentWithMax
            };
        }
    }
}
