using BWolf.Utilities.ObjectPooling;
using UnityEngine;

namespace BWolf.Examples.ObjectPooling
{
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        public bool CanTake
        {
            get { return !gameObject.activeInHierarchy; }
        }

        public void Return()
        {
            gameObject.SetActive(false);
        }

        public void OnTake()
        {
            gameObject.SetActive(true);
        }
    }
}