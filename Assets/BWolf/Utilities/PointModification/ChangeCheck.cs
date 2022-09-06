using System;

namespace BWolf.Utilities
{
    public readonly struct ChangeCheck
    {
        public bool Changed => _check != null && _initial != _check.Invoke();
        
        private readonly bool _initial;

        private readonly Func<bool> _check;

        public ChangeCheck(Func<bool> check)
        {
            _initial = check.Invoke();
            _check = check;
        }
    }
}
