using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ListPooling
{
    /// <summary>Default implementation for a gameobject list pool</summary>
    public class GameObjectListPool : ListPool<GameObject>
    {
        /// <summary>Creates an empty list with gameobjects. Either by getting it from the pool or by using new</summary>
        public override List<GameObject> Create()
        {
            List<GameObject> list;
            if (!bag.TryTake(out list))
            {
                list = new List<GameObject>();
            }
            return list;
        }

        /// <summary>Creates a list of given capacity with game objects. Either by getting it from the pool or by using new</summary>
        public override List<GameObject> Create(int capacity)
        {
            List<GameObject> list;
            if (!bag.TryTake(out list))
            {
                list = new List<GameObject>(capacity);
            }
            else
            {
                list.Capacity = capacity;
            }
            return list;
        }

        /// <summary>Creates a list using given collection with game objects. Either by getting it from the pool or by using new</summary>
        public override List<GameObject> Create(IEnumerable<GameObject> collection)
        {
            List<GameObject> list;
            if (!bag.TryTake(out list))
            {
                list = new List<GameObject>(collection);
            }
            else
            {
                list.AddRange(collection);
            }
            return list;
        }

        /// <summary>Disposes of given game object list</summary>
        public override void Dispose(List<GameObject> list)
        {
            list.Clear();
            bag.Add(list);
        }
    }
}