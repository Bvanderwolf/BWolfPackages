using System;
using System.Collections.Generic;

/*
 * TODO:
 * Add namespaces
 * Add assembly definitions
 * Add unit tests
 * Check inventory (IInventory interface?) compatability with PlayerData
 * Check statmodification compatability with PlayerData 
 */
namespace Bwolf.PlayerStats
{
    public class StatModifierBuilder
    {
        private readonly List<StatModifier> _modifiers;

        private readonly IStatRandomizer _baseRandomizer;

        public StatModifierBuilder(IStatRandomizer baseRandomizer = null)
        {
            _modifiers = new List<StatModifier>();
            _baseRandomizer = baseRandomizer;
        }

        public StatModifierBuilder New<T>(Action<T> constructor) where T : StatModifier
        {
            _modifiers.Add(StatModifier.New(constructor));
            return this;
        }

        public StatModifierBuilder New(Type type, Action<StatModifier> constructor)
        {
            _modifiers.Add(StatModifier.New(type, constructor));
            return this;
        }

        public StatModifierBuilder Random<T>(IStatRandomizer<T> randomizer = null) where T : StatModifier
        {
            if (randomizer == null && _baseRandomizer == null)
                throw new ArgumentException("Randomizer is null and no base randomizer set.");

            if (randomizer != null)
                _modifiers.Add(StatModifier.Random(randomizer));
            else
                _modifiers.Add(StatModifier.Random(typeof(T), _baseRandomizer));
            return this;
        }

        public StatModifierBuilder Random(Type type, IStatRandomizer randomizer = null)
        {
            if (randomizer == null && _baseRandomizer == null)
                throw new ArgumentException("Randomizer is null and no base randomizer set.");

            if (randomizer != null)
                _modifiers.Add(StatModifier.Random(type, randomizer));
            else
                _modifiers.Add(StatModifier.Random(type, _baseRandomizer));
            return this;
        }
    }
}