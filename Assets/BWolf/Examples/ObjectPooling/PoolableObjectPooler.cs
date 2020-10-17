using BWolf.Utilities.ObjectPooling;
using UnityEngine;

namespace BWolf.Examples.ObjectPooling
{
    public class PoolableObjectPooler : ObjectPooler<PoolableObject>
    {
        private static PoolableObjectPooler _instance;

        private static bool appIsQuitting;

        /// <summary>Use when overriding Awake to only execute Awake code if this is not a duplicate</summary>
        protected bool isDuplicate;

        public static PoolableObjectPooler Instance
        {
            get
            {
                if (appIsQuitting)
                {
                    //if this object is already destroyed during app closage return null to avoid MissingReferenceException
                    return null;
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                isDuplicate = true;
                Destroy(this.gameObject);
            }
            else
            {
                _instance = GetComponent<PoolableObjectPooler>();
                DontDestroyOnLoad(this.gameObject);
            }
        }

        public PoolableObject CreatePoolableObject()
        {
            return Take();
        }

        public void DestroyPoolableObject(PoolableObject poolableObject)
        {
            if (poolableObject != null)
            {
                poolableObject.Return();
            }
            else
            {
                Debug.LogWarning("Trying to destroy poolable object that is null");
            }
        }

        private PoolableObjectPooler()
        {
        }

        private void OnDestroy()
        {
            if (!isDuplicate)
            {
                //only set appIsQuitting flag if this is not a duplicate being destroyed
                appIsQuitting = true;
            }
        }
    }
}