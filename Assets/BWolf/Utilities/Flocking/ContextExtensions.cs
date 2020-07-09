using System.Collections.Generic;

namespace BWolf.Utilities.Flocking
{
    public static class ContextExtensions
    {
        public static List<UnitContext> Filtered(this List<UnitContext> context, int layerMask)
        {
            List<UnitContext> filtered = new List<UnitContext>();
            foreach (UnitContext item in context)
            {
                if (layerMask == (layerMask | (1 << item.Layer)))
                {
                    filtered.Add(item);
                }
            }
            return filtered;
        }
    }
}