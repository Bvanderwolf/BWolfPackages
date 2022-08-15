namespace Bwolf.PlayerStats
{
    /// <summary>
    /// Represents an interface for objects that randomize the
    /// value(s) of a stat modifier.
    /// </summary>
    public interface IStatRandomizer
    {
        /// <summary>
        /// Randomizes the value(s) of given stat modifier.
        /// </summary>
        /// <param name="statModifier">The stat modifier.</param>
        void Randomize(StatModifier statModifier);
    }
}