using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Examples.ObjectPooling
{
    public class PoolableObjectCreator : MonoBehaviour
    {
        [SerializeField]
        private KeyCode createKey = KeyCode.Return;

        [SerializeField]
        private KeyCode destroyKey = KeyCode.Escape;

        private List<PoolableObject> poolables = new List<PoolableObject>();
        private int indexOfLastPoolableCreated = -1;

        private void Update()
        {
            if (Input.GetKeyDown(createKey))
            {
                poolables.Add(PoolableObjectPooler.Instance.CreatePoolableObject());
                indexOfLastPoolableCreated = poolables.Count - 1;
            }

            if (Input.GetKeyDown(destroyKey) && indexOfLastPoolableCreated >= 0)
            {
                PoolableObjectPooler.Instance.DestroyPoolableObject(poolables[indexOfLastPoolableCreated]);
                poolables.RemoveAt(indexOfLastPoolableCreated);
                indexOfLastPoolableCreated = poolables.Count - 1;
            }
        }
    }
}