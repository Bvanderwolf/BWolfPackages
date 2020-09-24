// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using UnityEngine;

namespace BWolf.Behaviours.SingletonBehaviours
{
    /// <summary>
    /// A basic unity implementation of a singleton. This can be used with components that have inspector field that need to be set beforehand
    /// </summary>
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static bool appIsQuitting;

        public static T Instance
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
                Destroy(this.gameObject);
            }
            else
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            }
        }

        //make sure no instance of this object can be made using new keyword by making the constructor protected
        protected SingletonBehaviour()
        {
        }

        protected virtual void OnDestroy()
        {
            appIsQuitting = true;
        }
    }
}