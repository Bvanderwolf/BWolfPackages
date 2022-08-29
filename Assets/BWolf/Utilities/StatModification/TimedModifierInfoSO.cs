// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>scriptable object representing information about a timed stat modifier to be added into a stat system</summary>
    [CreateAssetMenu(fileName = "StatModifierInfo", menuName = "StatModification/TimedModifierInfo")]
    public class TimedModifierInfoSO : ModificationInfo
    {
        [Header("Timed Modification Settings")]
        [SerializeField, Tooltip("The time it takes for the modifier to modify the system in seconds")]
        public float time = 0.0f;
    }
}