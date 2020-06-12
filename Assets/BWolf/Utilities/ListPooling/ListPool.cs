using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>abstract class to be derived from by custom list pools</summary>
    public abstract class ListPool<T> : IListPool<T>
    {
        protected ConcurrentBag<List<T>> bag = new ConcurrentBag<List<T>>();

        public abstract List<T> Create();

        public abstract List<T> Create(int capacity);

        public abstract List<T> Create(IEnumerable<T> collection);

        public abstract void Dispose(List<T> list);
    }
}