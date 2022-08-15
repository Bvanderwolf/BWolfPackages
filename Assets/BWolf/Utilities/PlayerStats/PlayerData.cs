using UnityEngine;

namespace Bwolf.PlayerStats
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
        /// The player statistics of the player like health and/or mana points.
        /// </summary>
        public PlayerStats Stats => _stats;
    }
}
