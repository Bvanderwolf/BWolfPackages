    using BWolf.Gameplay;
using UnityEngine;

namespace BWolf.PlayerStatistics
{
    /// <summary>
    /// Holds information related to a player.
    /// </summary>
    [CreateAssetMenu(menuName = "PlayerStats/Data")]
    public class PlayerData : ScriptableObject
    {
        /// <summary>
        /// Holds statistics of the player like health and/or mana points.
        /// </summary>
        [SerializeField]
        private PlayerStats _stats;

        /// <summary>
        /// The statistics of the player like health and/or mana points.
        /// </summary>
        public PlayerStats Stats => _stats;

        /// <summary>
        /// The items held by the player like weapons and/or armor.
        /// </summary>
        public Inventory Items { get; set; } = new Inventory(1);
    }
}
