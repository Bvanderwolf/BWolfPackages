using System.Collections.Generic;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>interface to be implemented for list pooling</summary>
    public interface IListPool<T>
    {
        List<T> Create();

        void Dispose(List<T> list);
    }
}