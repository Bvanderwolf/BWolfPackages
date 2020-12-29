// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

namespace BWolf.Utilities.StatModification
{
    /// <summary>Base class for modifiers to derive from to modify a stat system</summary>
    public abstract class StatModifier
    {
        public string name { get; protected set; }

        /// <summary>
        /// Can this modifier stack with modifiers with the same name?
        /// </summary>
        public bool canStack { get; protected set; }

        /// <summary>
        /// Will this modifier increase or subtract value from the system?
        /// </summary>
        public bool increase { get; protected set; }

        /// <summary>
        /// Will this modifier modify current value or max value?
        /// </summary>
        public bool modifiesCurrent { get; protected set; }

        /// <summary>
        /// Does this modifier, when max is modified, also modify current?
        /// </summary>
        public bool modifiesCurrentWithMax { get; protected set; }

        /// <summary>To be implemented to make the stat system able to check whether to remove this modifier or not</summary>
        public abstract bool Finished { get; }

        /// <summary>To be implemented to make the stat system modify itself using this method</summary>
        public abstract void Modify(StatSystem system);
    }
}