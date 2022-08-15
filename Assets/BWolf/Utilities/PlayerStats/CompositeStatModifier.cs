using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bwolf.PlayerStats
{
    public class CompositeStatModifier : StatModifier
    {
        public class Builder
        {
            private readonly List<StatModifier> _modifiers;

            private readonly IStatRandomizer _baseRandomizer;

            public Builder(IStatRandomizer baseRandomizer = null)
            {
                _modifiers = new List<StatModifier>();
                _baseRandomizer = baseRandomizer;
            }

            public Builder AddNew<T>(Action<T> constructor) where T : StatModifier
            {
                _modifiers.Add(StatModifier.New(constructor));
                return this;
            }

            public Builder AddNew(Type type, Action<StatModifier> constructor)
            {
                _modifiers.Add(StatModifier.New(type, constructor));
                return this;
            }

            public Builder AddRandom<T>(IStatRandomizer<T> randomizer = null) where T : StatModifier
            {
                if (randomizer == null && _baseRandomizer == null)
                    throw new ArgumentException("Randomizer is null and no base randomizer set.");

                if (randomizer != null)
                    _modifiers.Add(StatModifier.Random(randomizer));
                else
                    _modifiers.Add(StatModifier.Random(typeof(T), _baseRandomizer));
                return this;
            }

            public Builder AddRandom(Type type, IStatRandomizer randomizer = null)
            {
                if (randomizer == null && _baseRandomizer == null)
                    throw new ArgumentException("Randomizer is null and no base randomizer set.");

                if (randomizer != null)
                    _modifiers.Add(StatModifier.Random(type, randomizer));
                else
                    _modifiers.Add(StatModifier.Random(type, _baseRandomizer));
                return this;
            }

            public CompositeStatModifier Build()
            {
                CompositeStatModifier instance = CreateInstance<CompositeStatModifier>();
                instance._modifiers = _modifiers.ToArray();
                return instance;
            }
        }

        [SerializeField]
        private StatModifier[] _modifiers;

        public override void Modify(PlayerStats stats)
        {
            for (int i = 0; i < _modifiers.Length; i++)
                _modifiers[i].Modify(stats);
        }

        public static Builder New(IStatRandomizer randomizer = null) => new Builder(randomizer);
    }
}