using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.PlayerStatistics
{
    /// <summary>
    /// Represents a composition of stat modifiers. This can be used to combine
    /// multiple modifiers into one.
    /// </summary>
    public class CompositeStatModifier : StatModifier
    {
        /// <summary>
        /// Provides methods to build a new composite stat modifier using
        /// the builder design pattern.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// The list of modifiers to be a composition.
            /// </summary>
            private readonly List<StatModifier> _modifiers;

            /// <summary>
            /// The base randomizer used for added stat modifiers with random values.
            /// </summary>
            private readonly IStatRandomizer _baseRandomizer;

            /// <summary>
            /// Creates a new builder instance, optionally using a stat randomizer
            /// to be used for all stat modifiers with random values.
            /// </summary>
            /// <param name="baseRandomizer">The base randomizer.</param>
            public Builder(IStatRandomizer baseRandomizer = null)
            {
                _modifiers = new List<StatModifier>();
                _baseRandomizer = baseRandomizer;
            }

            /// <summary>
            /// Adds a stat modifier to the composite.
            /// </summary>
            /// <param name="modifier">The stat modifier to add.</param>
            /// <returns>The builder instance.</returns>
            public Builder Add(StatModifier modifier)
            {
                if (modifier == null)
                    throw new ArgumentNullException(nameof(modifier));
                
                _modifiers.Add(modifier);
                return this;
            }

            /// <summary>
            /// Adds a new stat modifier to the composite.
            /// </summary>
            /// <param name="constructor">The constructor for the new stat modifier.</param>
            /// <typeparam name="T">The type of stat modifier.</typeparam>
            /// <returns>The builder instance.</returns>
            public Builder AddNew<T>(Action<T> constructor) where T : StatModifier
            {
                if (constructor == null)
                    throw new ArgumentNullException(nameof(constructor));
                
                _modifiers.Add(New(constructor));
                return this;
            }

            /// <summary>
            /// Adds a new stat modifier to the composite.
            /// </summary>
            /// <param name="constructor">The constructor for the new stat modifier.</param>
            /// <param name="type">The type of stat modifier.</param>
            /// <returns>The builder instance.</returns>
            public Builder AddNew(Type type, Action<StatModifier> constructor)
            {
                if (constructor == null)
                    throw new ArgumentNullException(nameof(constructor));
                
                _modifiers.Add(New(type, constructor));
                return this;
            }

            /// <summary>
            /// Adds a new stat modifier with random values to the composite.
            /// </summary>
            /// <param name="randomizer">The optional randomizer for the new stat modifier.</param>
            /// <typeparam name="T">The type of stat modifier.</typeparam>
            /// <returns>The builder instance.</returns>
            public Builder AddRandom<T>(IStatRandomizer<T> randomizer = null) where T : StatModifier
            {
                if (randomizer == null && _baseRandomizer == null)
                    throw new ArgumentException("Randomizer is null and no base randomizer set.");

                if (randomizer != null)
                    _modifiers.Add(Random(randomizer));
                else
                    _modifiers.Add(Random(typeof(T), _baseRandomizer));
                return this;
            }

            /// <summary>
            /// Adds a new stat modifier to the composite.
            /// </summary>
            /// <param name="randomizer">The optional randomizer for the new stat modifier.</param>
            /// <param name="type">The type of stat modifier.</param>
            /// <returns>The builder instance.</returns>
            public Builder AddRandom(Type type, IStatRandomizer randomizer = null)
            {
                if (randomizer == null && _baseRandomizer == null)
                    throw new ArgumentException("Randomizer is null and no base randomizer set.");

                if (randomizer != null)
                    _modifiers.Add(Random(type, randomizer));
                else
                    _modifiers.Add(Random(type, _baseRandomizer));
                return this;
            }

            /// <summary>
            /// Builds the composite, returning the new instance.
            /// </summary>
            /// <returns>The composite instance.</returns>
            public CompositeStatModifier Build()
            {
                if (_modifiers.Count == 0)
                    throw new InvalidOperationException("Invalid build :: No modifiers added to the composite.");
                
                CompositeStatModifier instance = CreateInstance<CompositeStatModifier>();
                instance._modifiers = _modifiers.ToArray();
                return instance;
            }
        }

        /// <summary>
        /// The array of modifiers, visible in the inspector.
        /// </summary>
        [SerializeField]
        private StatModifier[] _modifiers;

        /// <summary>
        /// Modifies the player stats using all modifiers in the composition.
        /// </summary>
        /// <param name="stats">The player stats to be modified.</param>
        public override void Modify(PlayerStats stats)
        {
            for (int i = 0; i < _modifiers.Length; i++)
                _modifiers[i].Modify(stats);
        }

        /// <summary>
        /// Returns a builder providing methods to build a new composite stat modifier using
        /// the builder design pattern.
        /// </summary>
        /// <param name="randomizer">The optional randomizer using for adding
        /// stat modifiers with random values.</param>
        /// <returns>The builder instance.</returns>
        public static Builder New(IStatRandomizer randomizer = null) => new Builder(randomizer);
    }
}