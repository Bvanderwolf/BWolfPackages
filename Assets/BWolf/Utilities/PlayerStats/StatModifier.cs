using System;
using UnityEngine;

namespace Bwolf.PlayerStats
{
    /// <summary>
    /// An abstract representation of a player statistic modifier.
    /// </summary>
    public abstract class StatModifier : ScriptableObject
    {
        /// <summary>
        /// Should modify the given player statistics.
        /// </summary>
        /// <param name="stats">The player statistics to modify.</param>
        public abstract void Modify(PlayerStats stats);

        /// <summary>
        /// Creates a new stat modifier. An optional constructor can be used to assign values.
        /// </summary>
        /// <param name="constructor">The constructor method of the stat modifier.</param>
        /// <typeparam name="T">The type of statistic modifier.</typeparam>
        /// <returns>The statistic modifier instance.</returns>
        public static T New<T>(Action<T> constructor = null) where T : StatModifier
        {
            T instance = CreateInstance<T>();
            constructor?.Invoke(instance);
            return instance;
        }

        /// <summary>
        /// Creates a new stat modifier. An optional constructor can be used to assign values.
        /// </summary>
        /// <param name="type">The type of statistic modifier.</param>
        /// <param name="constructor">The constructor method of the stat modifier.</param>
        /// <returns>The statistic modifier instance.</returns>
        public static StatModifier New(Type type, Action<StatModifier> constructor = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(StatModifier).IsAssignableFrom(type))
                throw new ArgumentException("Type does not derive from stat modifier.");

            StatModifier instance = (StatModifier)CreateInstance(type);
            constructor?.Invoke(instance);
            return instance;
        }

        /// <summary>
        /// Creates a new random stat modifier. A randomizer is needed to assign the random values.
        /// </summary>
        /// <param name="randomizer">The object implementing the randomizer interface.</param>
        /// <typeparam name="T">The type of statistic modifier.</typeparam>
        /// <returns>The statistic modifier instance.</returns>
        public static T Random<T>(IStatRandomizer<T> randomizer) where T : StatModifier
        {
            if (randomizer == null)
                throw new ArgumentNullException(nameof(randomizer));

            return New<T>(randomizer.Randomize);
        }

        /// <summary>
        /// Creates a new random stat modifier. A randomizer is needed to assign the random values.
        /// </summary>
        /// <param name="type">The type of statistic modifier.</param>
        /// <param name="randomizer">The object implementing the randomizer interface.</param>
        /// <returns>The statistic modifier instance.</returns>
        public static StatModifier Random(Type type, IStatRandomizer randomizer)
        {
            if (randomizer == null)
                throw new ArgumentNullException(nameof(randomizer));

            return New(type, randomizer.Randomize);
        }
    }
}
