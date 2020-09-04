// Created By: Benjamin van der Wolf
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Utilities.ShapeShifting
{
    /// <summary>
    /// Interface to be implemented by shapes that have templatability
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IShape<T> : ITemplatable<T>
    {
        GameObject Instantiate();
    }
}