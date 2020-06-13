using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>static class for optimal creation and disposing of lists</summary>
    public static class ListPoolService
    {
        private static Dictionary<Type, object> pools = new Dictionary<Type, object>
        {
             { typeof(GameObject), new GameObjectListPool() },
             { typeof(Transform),  new TransformListPool()  }
        };

        /// <summary>
        /// Creates a list of given type
        /// </summary>
        public static List<T> Create<T>() => ((ListPool<T>)pools[typeof(T)]).Create();

        /// <summary>
        /// Creates a list of given type with given capacity
        /// </summary>
        public static List<T> Create<T>(int capacity) => ((ListPool<T>)pools[typeof(T)]).Create(capacity);

        /// <summary>
        ///Creates a list of given type with given collection
        /// </summary>
        public static List<T> Create<T>(IEnumerable<T> collection) => ((ListPool<T>)pools[typeof(T)]).Create(collection);

        /// <summary>
        /// Disposes of list by returning it to the pool
        /// </summary>
        public static void Dispose<T>(List<T> list) => ((ListPool<T>)pools[typeof(T)]).Dispose(list);
    }
}