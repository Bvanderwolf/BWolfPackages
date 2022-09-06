using System;
using System.Collections.Generic;

namespace BWolf.ChangeSensitivity
{
    public struct ChangeSensitive<T>
    {
        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;

                _value = value;
                if (_actions.TryGetValue(_value, out Action<T> action))
                    action.Invoke(_value);
            }
        }

        private T _value;

        private readonly Dictionary<T, Action<T>> _actions;

        public ChangeSensitive(T value) : this(value, new Dictionary<T, Action<T>>())
        {
        }

        private ChangeSensitive(T value, Dictionary<T, Action<T>> actions)
        {
            _value = value;
            _actions = actions;
        }

        public ChangeSensitive<T> On(T onValue, Action<T> callback)
        {
            _actions[onValue] += callback;
            return new ChangeSensitive<T>(_value, _actions);
        }
        
        public static implicit operator T(ChangeSensitive<T> cs) => cs._value;
        
        public static implicit operator ChangeSensitive<T>(T value) => new ChangeSensitive<T>(value);
    }
}
