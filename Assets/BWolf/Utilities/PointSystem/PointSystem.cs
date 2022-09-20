using System;
using System.Collections.Generic;
using System.Linq;

namespace BWolf.StatSystems
{
    /// <summary>
    /// Represents a system which holds mutable point values.
    /// </summary>
    public class PointSystem
    {
        /// <summary>
        /// Holds the point names with their respective base values.
        /// </summary>
        private readonly Dictionary<string, BaseValue> _points = new Dictionary<string, BaseValue>();

        /// <summary>
        /// The mutators currently part of the system.
        /// </summary>
        private readonly List<IPointMutator> _mutators = new List<IPointMutator>();

        /// <summary>
        ///  Represents a combination of a base value with its current.
        /// </summary>
        private struct BaseValue
        {
            /// <summary>
            /// The initial base value.
            /// </summary>
            public readonly int value;

            /// <summary>
            /// The current value.
            /// </summary>
            public int current;

            /// <summary>
            /// Creates a new instance.
            /// </summary>
            /// <param name="value">The initial base value.</param>
            public BaseValue(int value)
            {
                this.value = value;
                this.current = value;
            }

            /// <summary>
            /// Resets the current value to the base value.
            /// </summary>
            public BaseValue Reset()
            {
                current = value;
                return this;
            }

            /// <summary>
            /// Adds value to the current.
            /// </summary>
            /// <param name="valueToBeAdded">The value to be added.</param>
            public BaseValue Add(int valueToBeAdded)
            {
                current += valueToBeAdded;
                return this;
            }
        }

        /// <summary>
        /// Represents data that modifies the value of a point
        /// by a given value.
        /// </summary>
        private readonly struct ValueMutator : IPointMutator
        {
            private readonly int _value;

            private readonly string[] _statistics;

            public ValueMutator(string statistic, int value)
            {
                _value = value;
                _statistics = new string[] { statistic };
            }
            
            public string[] GetPointNames() => _statistics;

            public int GetMutatedValue() => _value;
        }

        /// <summary>
        /// Represents data that modifies the value of a point
        /// by a given percentage.
        /// </summary>
        private readonly struct PercentageMutator : IPointMutator
        {
            private readonly float _percentage;

            private readonly string[] _statistics;

            private readonly PointSystem _system;

            public PercentageMutator(PointSystem system, string statistic, float percentage)
            {
                _percentage = percentage;
                _statistics = new string[] { statistic };
                _system = system;
            }

            public string[] GetPointNames() => _statistics;

            public int GetMutatedValue()
            {
                int value = _system.GetValue(_statistics[0]);
                return (int)(value * _percentage);
            }
        }

        /// <summary>
        /// Gets or sets the base value of a point in the system.
        /// </summary>
        /// <param name="pointName">The name of the point.</param>
        public int this[string pointName]
        {
            get => GetBase(pointName);
            set => SetBase(pointName, value);
        }
        
        /// <summary>
        /// Sets the base value of a point in the system.
        /// </summary>
        /// <param name="pointName">The name of the point.</param>
        /// <param name="value">The new base value.</param>
        public void SetBase(string pointName, int value)
        {
            _points[pointName] = new BaseValue(value);
        }

        /// <summary>
        /// Gets the base value of a point in the system.
        /// </summary>
        /// <param name="pointName">The name of the point.</param>
        /// <returns>The base value.</returns>
        public int GetBase(string pointName)
        {
            if (!_points.TryGetValue(pointName, out BaseValue baseValue))
                throw new ArgumentException($"Point {pointName} has not been added yet.");

            return baseValue.value;
        }

        /// <summary>
        /// Gets the current value of a point in the system.
        /// </summary>
        /// <param name="pointName">The name of the point.</param>
        /// <returns>The current value.</returns>
        public int GetValue(string pointName)
        {
            if (!_points.TryGetValue(pointName, out BaseValue baseValue))
                throw new ArgumentException($"Point {pointName} has not been added yet.");

            return baseValue.current;
        }

        /// <summary>
        /// Mutates a point by a percentage value.
        /// The percentage is calculated from the points base value.
        /// </summary>
        /// <param name="pointName">The point name.</param>
        /// <param name="percentage">The percentage value.</param>
        public void Mutate(string pointName, float percentage) =>
            Mutate(new PercentageMutator(this, pointName, percentage));

        /// <summary>
        /// Mutates a point by a percentage value.
        /// </summary>
        /// <param name="pointName">The point name.</param>
        /// <param name="value">The value.</param>
        public void Mutate(string pointName, int value) 
            => Mutate(new ValueMutator(pointName, value));

        /// <summary>
        /// Mutates the system using a mutator.
        /// </summary>
        /// <param name="mutator">The point mutator.</param>
        public void Mutate(IPointMutator mutator)
        {
            _mutators.Add(mutator);
            Refresh();
        }

        /// <summary>
        /// Refreshes the points using the stored mutators.
        /// </summary>
        private void Refresh()
        {
            // First reset all points to their base values.
            string[] pointNames = _points.Keys.ToArray();
            foreach (string point in pointNames)
                _points[point] = _points[point].Reset();

            // Then calculate the total values for each point based on the current set of mutators.
            KeyValuePair<string, int>[] mutateTotals = CalculateTotals(_mutators);
            for (int i = 0; i < mutateTotals.Length; i++)
            {
                KeyValuePair<string, int> total = mutateTotals[i];
                _points[total.Key] = _points[total.Key].Add(total.Value);
            }
        }

        /// <summary>
        /// Calculates totals of a list of point mutators.
        /// </summary>
        /// <param name="mutators">The list of point mutators to get the totals for.</param>
        /// <returns>The totals for each point.</returns>
        private static KeyValuePair<string, int>[] CalculateTotals(List<IPointMutator> mutators)
        {
            Dictionary<string, int> totals = new Dictionary<string, int>();
            
            // Flatten the list of mutators based on the statistic names as one mutator can mutate many statistics.
            KeyValuePair<string, int>[] flattened = mutators
                .SelectMany(m => m.GetPointNames()
                    .Select(n => new KeyValuePair<string, int>(n, m.GetMutatedValue())))
                .ToArray();

            // Add the value to mutate to the total for each statistic.
            for (int i = 0; i < flattened.Length; i++)
            {
                KeyValuePair<string, int> mutator = flattened[i];
                totals[mutator.Key] = totals.ContainsKey(mutator.Key) 
                    ? totals[mutator.Key] + mutator.Value 
                    : mutator.Value;
            }

            return totals.ToArray();
        }
    }
}
