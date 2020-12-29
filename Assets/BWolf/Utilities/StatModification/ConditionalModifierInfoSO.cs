// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace BWolf.Utilities.StatModification
{
    /// <summary>scriptable object representing information about a conditional modifier to be added into a stat system</summary>
    [CreateAssetMenu(fileName = "StatModifierInfo", menuName = "StatModification/ConditionalModifierInfo")]
    public class ConditionalModifierInfoSO : ModifierInfoSO
    {
        [Header("Conditional Modifier Settings")]
        public SecondPassedEvent OnSecondsPassed;

        public ModificationEndCondition StopCondition;
    }

    public delegate bool ModificationEndCondition();

    [Serializable]
    public class SecondPassedEvent : UnityEvent<string, int> { }
}