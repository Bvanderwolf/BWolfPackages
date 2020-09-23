// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.1
//----------------------------------

using BWolf.Behaviours;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>static class used for creating list pools</summary>
    public class ListPool<T> : SingletonBehaviour<ListPool<T>>
    {
        private ConcurrentBag<List<T>> bag = new ConcurrentBag<List<T>>();

        /// <summary>Creates an empty list with objects of Type type.</summary>
        public List<T> Create()
        {
            List<T> list;
            if (!bag.TryTake(out list))
            {
                list = new List<T>();
            }

            return list;
        }

        /// <summary>Creates a list of given capacity with objects of Type type</summary>
        public List<T> Create(int capacity)
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
        public List<T> Create(IEnumerable<T> collection)
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
        public void Dispose(List<T> list)
        {
            list.Clear();
            bag.Add(list);
        }
    }
}