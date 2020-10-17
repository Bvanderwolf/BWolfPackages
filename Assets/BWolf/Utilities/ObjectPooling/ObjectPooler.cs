using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.ObjectPooling
{
    public class ObjectPooler<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private GameObject prefab = null;

        private List<T> pool = new List<T>();

        protected T Take()
        {
            if (!TryGetPooledObject(out T instance))
            {
                instance = GameObject.Instantiate(prefab).GetComponent<T>();
                instance.OnTake();
                pool.Add(instance);
            }

            return instance;
        }

        protected void Return(T poolable)
        {
            poolable.Return();
        }

        private bool TryGetPooledObject(out T poolable)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].CanTake)
                {
                    poolable = pool[i];
                    poolable.OnTake();
                    return true;
                }
            }

            poolable = null;
            return false;
        }
    }
}