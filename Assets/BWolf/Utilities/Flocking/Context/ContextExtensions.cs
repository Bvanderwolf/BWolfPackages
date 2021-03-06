﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System.Collections.Generic;

namespace BWolf.Utilities.Flocking.Context
{
    /// <summary>static class providing extensions for a list of context items</summary>
    public static class ContextExtensions
    {
        /// <summary>Returns a new list with context item matching the given layer mask</summary>
        public static List<ContextItem> FilteredToLayer(this List<ContextItem> context, int layerMask)
        {
            List<ContextItem> filtered = new List<ContextItem>();
            foreach (ContextItem item in context)
            {
                if (layerMask == (layerMask | (1 << item.Layer)))
                {
                    filtered.Add(item);
                }
            }
            return filtered;
        }

        public static List<ContextItem> ExcludingLayer(this List<ContextItem> context, int layerMask)
        {
            List<ContextItem> filtered = new List<ContextItem>();
            foreach (ContextItem item in context)
            {
                if (layerMask != (layerMask | (1 << item.Layer)))
                {
                    filtered.Add(item);
                }
            }
            return filtered;
        }
    }
}