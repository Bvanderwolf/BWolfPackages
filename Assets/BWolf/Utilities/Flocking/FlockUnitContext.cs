using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public struct FlockUnitContext
    {
        public Transform ContextTransform;
        public int Layer;

        public static FlockUnitContext Create(Transform contextItem, int layer)
        {
            FlockUnitContext context;
            context.ContextTransform = contextItem;
            context.Layer = layer;
            return context;
        }
    }
}