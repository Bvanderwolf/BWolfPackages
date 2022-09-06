using UnityEngine;

namespace BWolf.PointModifications
{
    public class TimedPointModifier : PointModifier
    {
        public float time;

        public float interval;

        private float _timePassed;

        private float _intervalPassed;

        private int _valueModified;

        private int _currentValue;

        public override bool IsFinished => _timePassed >= time;

        public TimedPointModifier(string name, int value, float time, float interval = 0f) : base(name, value)
        {
            this.time = time;
            this.interval = interval;
        }
        
        public TimedPointModifier(string name, int value, float time, bool modifiesCurrent, float interval = 0f) : base(name, value, modifiesCurrent)
        {
            this.time = time;
            this.interval = interval;
        }
        
        public TimedPointModifier(
            string name, 
            int value, 
            float time, 
            bool modifiesCurrent, 
            float interval = 0f,
            bool modifiesCurrentWithMax = false) 
            : base(name, value, modifiesCurrent, modifiesCurrentWithMax)
        {
            this.time = time;
            this.interval = interval;
        }

        public override PointModification Modify(PointSystem pointSystem)
        {
            if (time <= 0)
                return new PointModification(value);
            
            _timePassed += Time.deltaTime;
            _intervalPassed += Time.deltaTime;
            _currentValue = (int)(_timePassed / time * value);

            if (_currentValue == _valueModified || (_intervalPassed < interval && !IsFinished))
                return PointModification.None;

            int modificationValue = GetModificationValue();
            HandleOvershot(ref modificationValue);
            _valueModified += modificationValue;
            _intervalPassed = 0;

            return new PointModification
            {
                value = modificationValue,
                modifiesCurrent = modifiesCurrent,
                modifiesCurrentWithMax = modifiesCurrentWithMax
            };
        }

        private int GetModificationValue()
        {
            int difference = Mathf.Abs(_currentValue - _valueModified);
            
            if (!AddsValue)
                difference *= -1;

            return difference;
        }

        /// <summary>
        /// Overshot can happen with high values and small times.
        /// </summary>
        /// <param name="modificationValue"></param>
        private void HandleOvershot(ref int modificationValue)
        {
            int overshot;
            
            if (AddsValue && _currentValue > value)
            {
                overshot = _currentValue - value;
                modificationValue -= overshot;
            }
            else if (!AddsValue && _currentValue < value)
            {
                overshot = _currentValue + value;
                modificationValue += overshot;
            }
        }
    }
}
