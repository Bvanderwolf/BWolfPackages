namespace Bwolf.PlayerStats
{
   public interface IStatRandomizer<in T> where T : StatModifier
   {
      void Randomize(T statModifier);
   }
}
