﻿using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    /// <summary>Representation of a Cotext</summary>
    public struct ContextItem
    {
        public Transform ContextTransform;
        public int Layer;

        public static ContextItem Create(Transform contextItem, int layer)
        {
            ContextItem context;
            context.ContextTransform = contextItem;
            context.Layer = layer;
            return context;
        }
    }
}