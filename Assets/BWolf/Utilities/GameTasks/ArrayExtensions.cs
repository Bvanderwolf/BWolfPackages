using System;

namespace BWolf.GameTasks
{
    /// <summary>
    /// Provides additional methods for modifying the state of an array.
    /// </summary>
    public static class ArrayExtensions 
    {
        /// <summary>
        /// Increments the array adding a new given element to it.
        /// Equivalent of List<T>.Add() method.
        /// </summary>
        /// <param name="array">The array to increment.</param>
        /// <param name="element">The element to add.</param>
        /// <typeparam name="T">The type of array.</typeparam>
        public static void Increment<T>(this T[] array, T element)
        {
            int length = array.Length;
            Array.Resize(ref array, length);
            array[length] = element;
        }

        /// <summary>
        /// Removes an element of the array at a given index.
        /// Equivalent of List<T>.RemoveAt() method.
        /// </summary>
        /// <param name="array">The array to remove an element from.</param>
        /// <param name="index">The index at which to remove an element.</param>
        /// <typeparam name="T">The type of array.</typeparam>
        public static void RemoveAt<T>(this T[] array, int index)
        {
            int newLength = array.Length - 1;
            T last = array[newLength];
            Array.Resize(ref array, newLength);
            array[index] = last;
        }
    }
}