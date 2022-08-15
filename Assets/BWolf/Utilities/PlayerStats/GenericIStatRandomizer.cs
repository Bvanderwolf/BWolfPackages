namespace BWolf.PlayerStatistics
{
   /// <summary>
   /// Represents an interface for objects that randomize the
   /// value(s) of a stat modifier of type T.
   /// </summary>
   /// <typeparam name="T">The type of stat modifier.</typeparam>
   public interface IStatRandomizer<in T> where T : StatModifier
   {
      /// <summary>
      /// Randomizes the value(s) of given stat modifier.
      /// </summary>
      /// <param name="statModifier">The stat modifier.</param>
      void Randomize(T statModifier);
   }
}
