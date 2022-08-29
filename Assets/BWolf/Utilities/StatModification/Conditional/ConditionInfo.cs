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
    public class ConditionInfo : ModificationInfo
    {
        [Header("Conditional Modifier Settings")]
        [Min(0f)]
        public float interval = 1f;
        
        public SecondPassedEvent OnSecondsPassed;

        public Func<bool> StopCondition;
    }
}