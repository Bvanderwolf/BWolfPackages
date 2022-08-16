using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.PlayerStatistics
{
    /// <summary>
    /// Represents the player statistics.
    /// </summary>
    [Serializable]
    public class PlayerStats
    {
        /// <summary>
        /// The array of player statistics visible in the inspector.
        /// </summary>
        [SerializeField]
        private List<PlayerStat> _list = new List<PlayerStat>();

        /// <summary>
        /// The amount of player statistics.
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// The amount of player stat modifiers.
        /// </summary>
        public int ModifierCount => _modifiers.Count;
        
        /// <summary>
        /// The modifiers applied to the player statistics.
        /// </summary>
        private readonly List<StatModifier> _modifiers = new List<StatModifier>();

        /// <summary>
        /// Adds a new player statistic to current collection.
        /// </summary>
        /// <param name="statistic">The statistic to add.</param>
        public void Add(PlayerStat statistic)
        {
            if (statistic == null)
                throw new ArgumentNullException(nameof(statistic));
            
            _list.Add(statistic);
        }
        
        /// <summary>
        /// Removes a player statistic from the current collection.
        /// </summary>
        /// <param name="statistic">The statistic to remove.</param>
        public void Remove(PlayerStat statistic)
        {
            if (statistic == null)
                throw new ArgumentNullException(nameof(statistic));
            
            _list.Remove(statistic);
        }
        
        /// <summary>
        /// Modifies the player statistics by adding the given stat modifier.
        /// </summary>
        /// <param name="modifier">The stat modifier.</param>
        public void Modify(StatModifier modifier)
        {
            if (modifier == null)
                throw new ArgumentNullException(nameof(modifier));

            _modifiers.Add(modifier);
            Refresh();
        }

        /// <summary>
        /// Un modifies the player statistics by removing the given stat modifier. 
        /// </summary>
        /// <param name="modifier">The stat modifier.</param>
        public void UnModify(StatModifier modifier)
        {
            if (modifier == null)
                throw new ArgumentNullException(nameof(modifier));

            _modifiers.Remove(modifier);
            Refresh();
        }

        /// <summary>
        /// Un modifies the player statistics by removing all modifiers.
        /// </summary>
        public void UnModify() => _modifiers.Clear();

        /// <summary>
        /// Returns statistics from the player statistics of type T.
        /// Will return an empty array if the statistics are not found.
        /// </summary>
        /// <typeparam name="T">The type of player statistic.</typeparam>
        /// <returns>The player statistics.</returns>
        public T[] GetMultiple<T>() where T : PlayerStat
        {
            List<T> list = new List<T>();
            for (int i = 0; i < _list.Count; i++)
                if (_list[i] is T statOfTypeT)
                    list.Add(statOfTypeT);
            return list.ToArray();
        }

        /// <summary>
        /// Returns a statistic from the player statistics of type T.
        /// Will return null if the statistic is not found.
        /// </summary>
        /// <typeparam name="T">The type of player statistic.</typeparam>
        /// <returns>The player statistic.</returns>
        public T Get<T>() where T : PlayerStat
        {
            for (int i = 0; i < _list.Count; i++)
                if (_list[i] is T statOfTypeT)
                    return statOfTypeT;
            return null;
        }

        /// <summary>
        /// Returns a statistic from the player statistics of type T with given stat name.
        /// Will return null if the statistic is not found.
        /// </summary>
        /// <typeparam name="T">The type of player statistic.</typeparam>
        /// <returns>The player statistic.</returns>
        public T Get<T>(string statName) where T : PlayerStat
        {
            for (int i = 0; i < _list.Count; i++)
            {
                PlayerStat stat = _list[i];
                if (stat.name == statName && stat is T statOfTypeT)
                    return statOfTypeT;
            }

            return null;
        }

        /// <summary>
        /// Returns a statistic from the player statistics with given stat name.
        /// Will return null if the statistic is not found.
        /// </summary>
        /// <returns>The player statistic.</returns>
        public PlayerStat Get(string statName)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                PlayerStat stat = _list[i];
                if (stat.name == statName)
                    return stat;
            }

            return null;
        }

        /// <summary>
        /// Refreshes the player statistics by resetting all statistics to their
        /// base value and then re-modifying them using the currently stored modifiers.
        /// </summary>
        private void Refresh()
        {
            for (int i = 0; i < _list.Count; i++)
                _list[i].ResetToBaseValue();

            for (int i = 0; i < _modifiers.Count; i++)
                _modifiers[i].Modify(this);
        }
    }
}
