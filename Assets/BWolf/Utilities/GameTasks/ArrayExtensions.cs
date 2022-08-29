using System;

namespace BWolf.GameTasks
{
    public static class ArrayExtensions 
    {
        public static void Increment<T>(this T[] array, T element)
        {
            int length = array.Length;
            Array.Resize(ref array, length);
            array[length] = element;
        }

        public static void RemoveAt<T>(this T[] array, int index)
        {
            int newLength = array.Length - 1;
            T last = array[newLength];
            Array.Resize(ref array, newLength);
            array[index] = last;
        }
    }
}