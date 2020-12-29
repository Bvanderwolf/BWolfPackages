// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>scriptable object representing all generic information about a modifier to be added into a stat system</summary>
    public abstract class ModifierInfoSO : ScriptableObject
    {
        [Header("General Settings")]
        [Min(0), Tooltip("Amount of value it will modify")]
        public int Value;

        [Tooltip("Can this modifier stack with modifiers with the same name?")]
        public bool CanStack;

        [Tooltip("Will this modifier increase or subtract")]
        public bool Increase;

        [Tooltip("Will this modifier modify current value or max value?")]
        public bool ModifiesCurrent;

        [Tooltip("Does this modifier, when max is modified, also modify current?")]
        public bool ModifiesCurrentWithMax;
    }
}