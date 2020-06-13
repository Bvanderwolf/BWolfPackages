using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>abstract class to be derived from by custom list pools</summary>
    public class ListPool<T> : IListPool<T>
    {
        public static readonly ListPool<T> Instance = new ListPool<T>();

        protected ConcurrentBag<List<T>> bag = new ConcurrentBag<List<T>>();

        /// <summary>Creates an empty list with gameobjects.</summary>
        public List<T> Create()
        {
            List<T> list;
            if (!bag.TryTake(out list))
            {
                list = new List<T>();
            }
            return list;
        }

        /// <summary>Creates a list of given capacity with game objects.</summary>
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

        /// <summary>Creates a list using given collection with game objects.</summary>
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

        /// <summary>Disposes of given game object list</summary>
        public void Dispose(List<T> list)
        {
            list.Clear();
            bag.Add(list);
        }
    }
}