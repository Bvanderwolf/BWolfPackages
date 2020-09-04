// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

namespace BWolf.Utilities.ShapeShifting
{
    /// <summary>
    /// Interface to be implemented by objects that need templatability
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITemplatable<T>
    {
        T GetTemplate();
    }
}