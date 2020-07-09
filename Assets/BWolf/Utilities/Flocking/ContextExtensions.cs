using System.Collections.Generic;

namespace BWolf.Utilities.Flocking
{
    public static class ContextExtensions
    {
        public static List<FlockUnitContext> Filtered(this List<FlockUnitContext> context, int layerMask)
        {
            List<FlockUnitContext> filtered = new List<FlockUnitContext>();
            foreach (FlockUnitContext item in context)
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