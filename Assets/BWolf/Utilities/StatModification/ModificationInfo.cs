// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using UnityEngine;
using UnityEngine.Serialization;

namespace BWolf.Utilities.StatModification
{
    /// <summary>scriptable object representing all generic information about a modifier to be added into a stat system</summary>
    public abstract class ModificationInfo : ScriptableObject
    {
        [FormerlySerializedAs("Value")]
        [Header("General Settings")]
        [Min(0), Tooltip("Amount of value it will modify")]
        public int value;

        [FormerlySerializedAs("CanStack")]
        [Tooltip("Can this modifier stack with modifiers with the same name?")]
        public bool canStack;

        [FormerlySerializedAs("Increase")]
        [Tooltip("Will this modifier increase or subtract")]
        public bool increasesValue;

        [FormerlySerializedAs("ModifiesCurrent")]
        [Tooltip("Will this modifier modify current value or max value?")]
        public bool modifiesCurrent;

        [FormerlySerializedAs("ModifiesCurrentWithMax")]
        [Tooltip("Does this modifier, when max is modified, also modify current?")]
        public bool modifiesCurrentWithMax;
    }
}