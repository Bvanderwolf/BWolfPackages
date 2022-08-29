// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

namespace BWolf.Utilities.StatModification
{
    /// <summary>
    /// An abstract representation of a class that modifies a point statistic.
    /// </summary>
    public abstract class PointStatModifier
    {
        /// <summary>
        /// The name of the modifier.
        /// Important when stacking modifiers.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Whether this modifier can stack with modifiers of the same name.
        /// </summary>
        public bool IsStackable { get; protected set; }

        /// <summary>
        /// Whether this modifier will increase the value of the point stat system it modifies.
        /// </summary>
        public bool IncreasesValue { get; protected set; }

        /// <summary>
        /// Whether this modifier will update the current of maximum value of the point stat system;
        /// </summary>
        public bool ModifiesCurrent { get; protected set; }

        /// <summary>
        /// Whether this modifier will update the current value if the maximum value is modified.
        /// </summary>
        public bool ModifiesCurrentWithMax { get; protected set; }

        /// <summary>
        /// Should return whether the modifier has finished its modification to the point stat system.
        /// </summary>
        public abstract bool Finished { get; }

        /// <summary>
        /// Should update the given point stat system.
        /// </summary>
        /// <param name="system">The point stat system to modify.</param>
        public abstract void Modify(PointStatSystem system);
    }
}