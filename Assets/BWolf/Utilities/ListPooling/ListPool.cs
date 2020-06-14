using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>static class used for creating list pools</summary>
    public static class ListPool<T>
    {
        private static ConcurrentBag<List<T>> bag = new ConcurrentBag<List<T>>();

        /// <summary>Creates an empty list with objects of Type type.</summary>
        public static List<T> Create()
        {
            List<T> list;
            if (!bag.TryTake(out list))
            {
                list = new List<T>();
            }

            return list;
        }

        /// <summary>Creates a list of given capacity with objects of Type type</summary>
        public static List<T> Create(int capacity)
        {
            List<T> list;
            if (!bag.TryTake(out list))
            {
                list = new List<T>(capacity);
            }
            else
            {
                list.Capacity = capacity;
            }
            return list;
        }

        /// <summary>Creates a list using given collection with objects of Type type.</summary>
        public static List<T> Create(IEnumerable<T> collection)
        {
            List<T> list;
            if (!bag.TryTake(out list))
            {
                list = new List<T>(collection);
            }
            else
            {
                list.AddRange(collection);
            }
            return list;
        }

        /// <summary>Disposes of given objects of type type</summary>
        public static void Dispose(List<T> list)
        {
            list.Clear();
            bag.Add(list);
        }
    }
}