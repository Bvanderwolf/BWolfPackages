using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public struct UnitContext
    {
        public Transform ContextTransform;
        public int Layer;

        public static UnitContext Create(Transform contextItem, int layer)
        {
            UnitContext context;
            context.ContextTransform = contextItem;
            context.Layer = layer;
            return context;
        }
    }
}