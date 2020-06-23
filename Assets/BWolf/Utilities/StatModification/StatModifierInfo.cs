using UnityEngine;

namespace BWolf.Utilities.StatModification
{
    /// <summary>serializable strcuture used to wrap all generic information about a modifier to be added into a stat system</summary>
    [System.Serializable]
    public struct StatModifierInfo
    {
        [Tooltip("Identifier for stat modifier")]
        public string Name;

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

        /// <summary>Creates a new StatModifierInfo instance using only name, value and an increase flag</summary>
        public static StatModifierInfo Create(string name, int value, bool increase)
        {
            StatModifierInfo info;
            info.Name = name;
            info.Value = value;
            info.CanStack = false;
            info.Increase = increase;
            info.ModifiesCurrent = true;
            info.ModifiesCurrentWithMax = false;
            return info;
        }

        /// <summary>Creates a new StatModifierInfo instance using name, value, an increase flag and an canstack flag</summary>
        public static StatModifierInfo Create(string name, int value, bool increase, bool canStack)
        {
            StatModifierInfo info;
            info.Name = name;
            info.Value = value;
            info.CanStack = canStack;
            info.Increase = increase;
            info.ModifiesCurrent = true;
            info.ModifiesCurrentWithMax = false;
            return info;
        }

        /// <summary>Creates a new StatModifierInfo instance using name, value, an increase flag, an can stack flag and an modifies current flag</summary>
        public static StatModifierInfo Create(string name, int value, bool increase, bool canStack, bool modifiesCurrent)
        {
            StatModifierInfo info;
            info.Name = name;
            info.Value = value;
            info.CanStack = canStack;
            info.Increase = increase;
            info.ModifiesCurrent = modifiesCurrent;
            info.ModifiesCurrentWithMax = false;
            return info;
        }

        /// <summary>Creates a new StatModifierInfo instance setting all members using given arguments</summary>
        public static StatModifierInfo Create(string name, int value, bool increase, bool canStack, bool modifiesCurrent, bool modifiesCurrentWithMax)
        {
            StatModifierInfo info;
            info.Name = name;
            info.Value = value;
            info.CanStack = canStack;
            info.Increase = increase;
            info.ModifiesCurrent = modifiesCurrent;
            info.ModifiesCurrentWithMax = modifiesCurrentWithMax;
            return info;
        }
    }
}