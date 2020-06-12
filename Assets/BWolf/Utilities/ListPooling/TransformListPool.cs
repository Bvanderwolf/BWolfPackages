using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>Default implementation for a transform list pool</summary>
    public class TransformListPool : ListPool<Transform>
    {
        /// <summary>Creates a empty list with transforms. Either by getting it from the pool or by using new</summary>
        public override List<Transform> Create()
        {
            List<Transform> list;
            if (!bag.TryTake(out list))
            {
                list = new List<Transform>();
            }
            return list;
        }

        /// <summary>Creates a list of given capacity with transforms. Either by getting it from the pool or by using new</summary>
        public override List<Transform> Create(int capacity)
        {
            List<Transform> list;
            if (!bag.TryTake(out list))
            {
                list = new List<Transform>(capacity);
            }
            else
            {
                list.Capacity = capacity;
            }
            return list;
        }

        /// <summary>Creates a list using given collection with transforms. Either by getting it from the pool or by using new</summary>
        public override List<Transform> Create(IEnumerable<Transform> collection)
        {
            List<Transform> list;
            if (!bag.TryTake(out list))
            {
                list = new List<Transform>(collection);
            }
            else
            {
                list.AddRange(collection);
            }
            return list;
        }

        /// <summary>Disposes of given transforms list</summary>
        public override void Dispose(List<Transform> list)
        {
            list.Clear();
            bag.Add(list);
        }
    }
}