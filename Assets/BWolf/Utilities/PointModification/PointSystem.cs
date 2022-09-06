using System;
using System.Collections.Generic;
using BWolf.Utilities;
using UnityEngine;

namespace BWolf.PointModifications
{
    // TODO: override system instead of queue -> if the modifier has the same name, the modifier is overriden.
    [Serializable]
    public class PointSystem
    {
        public const int DEFAULT_MAX = 100;
        
        [SerializeField, Min(0)]
        private int _max = DEFAULT_MAX;

        [SerializeField]
        private int _current;

        public event Action<SystemEvent> Event;

        public int Max => _max;

        public int Current => _current;

        public bool HasModifiers => _modifiers.Count != 0;
        
        
        private readonly List<PointModifier> _modifiers = new List<PointModifier>();

        private ChangeCheck _increase;

        private ChangeCheck _decrease;

        private ChangeCheck _isAtMax;

        private ChangeCheck _isAtZero;

        public PointSystem() : this(DEFAULT_MAX)
        {
        }
        
        public PointSystem(int current, int max = DEFAULT_MAX)
        {
            _max = max;
            _current = current;
            _current = Mathf.Clamp(current, 0, _max);
        }

        public void Update()
        {
            if (!HasModifiers)
                return;
            
            BeginChangeChecks();
            {
                for (int i = _modifiers.Count - 1; i >= 0; i--)
                {
                    PointModifier modifier = _modifiers[i];
                    ApplyModification(modifier.Modify(this));
                    if (modifier.IsFinished)
                        RemoveModifierAt(i);
                }
            }
            EndChangeChecks();
        }

        public void Clear()
        {
            for (int i = _modifiers.Count - 1; i >= 0; i--)
                RemoveModifierAt(i);
        }

        public void Clear(string modifierName)
        {
            for (int i = _modifiers.Count - 1; i >= 0; i--)
                if (_modifiers[i].name == modifierName)
                    RemoveModifierAt(i);
        }

        public void Remove(string modifierName, int? count = null)
        {
            int removeCount = count ?? 1;
            
            for (int i = _modifiers.Count - 1; i >= 0; i--)
            {
                if (_modifiers[i].name == modifierName)
                {
                    RemoveModifierAt(i);
                    removeCount--;
                }

                if (removeCount == 0)
                    break;
            }
        }

        public void Add(PointModifier modifier)
        {
            // First try overriding an existing modifier with the same name.
            for (int i = 0; i < _modifiers.Count; i++)
            {
                if (_modifiers[i].name != modifier.name)
                    continue;
                
                _modifiers[i] = modifier;
                return;
            }
            
            // If the modifier is new, set a new change check based on whether
            // the modifier adds or removes value.
            if (modifier.AddsValue)
                _increase = new ChangeCheck(IsIncreasing);
            else
                _decrease = new ChangeCheck(IsDecreasing);
            
            _modifiers.Add(modifier);
        }

        public bool IsAtMax() => _current == _max;

        public void SetToMax() => _current = _max;

        public bool IsAtZero() => _current == 0;

        public void SetToZero() => _current = 0;

        public bool IsIncreasing()
        {
            for (int i = 0; i < _modifiers.Count; i++)
                if (_modifiers[i].AddsValue)
                    return true;

            return false;
        }

        public bool IsDecreasing()
        {
            for (int i = 0; i < _modifiers.Count; i++)
                if (!_modifiers[i].AddsValue)
                    return true;

            return false;
        }

        private void BeginChangeChecks()
        {
            _isAtMax = new ChangeCheck(IsAtMax);
            _isAtZero = new ChangeCheck(IsAtZero);
        }

        private void EndChangeChecks()
        {
            if (_isAtMax.Changed)
                Event?.Invoke(SystemEvent.REACHED_MAX);
            
            if (_isAtZero.Changed)
                Event?.Invoke(SystemEvent.REACHED_ZERO);
            
            if (_increase.Changed)
                Event?.Invoke(SystemEvent.INCREASE_START);
            
            if (_decrease.Changed)
                Event?.Invoke(SystemEvent.DECREASE_START);
        }

        private void ApplyModification(PointModification modification)
        {
            if (modification.modifiesCurrent)
            {
               ModifyCurrent();
            }
            else
            {
                _max += modification.value;
                if (modification.modifiesCurrentWithMax)
                    ModifyCurrent();
            }

            void ModifyCurrent()
            {
                _current += modification.value;
                _current = Mathf.Clamp(_current, 0, _max);
            }
        }

        private void RemoveModifierAt(int index)
        {
            // Determine whether the modifier to remove adds or removes value. Set up a predicate
            // to look whether any other modifier left in the system does the same.
            bool addsValue = _modifiers[index].AddsValue;
            Func<PointModifier, bool> predicate = m => m.AddsValue == addsValue;

            // After removal of the modifier, use the predicate to return the function if any other
            // modifier still in the system is doing the same.
            _modifiers.RemoveAt(index);
            for (int i = 0; i < _modifiers.Count; i++)
                if (predicate.Invoke(_modifiers[i]))
                    return;

            if (addsValue)
            {
                _increase = default;
                Event?.Invoke(SystemEvent.INCREASE_STOP);
            }
            else
            {
                _decrease = default;
                Event?.Invoke(SystemEvent.DECREASE_STOP);
            }
        }
    }
}
