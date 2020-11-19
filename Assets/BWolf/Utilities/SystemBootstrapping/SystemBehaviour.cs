using UnityEngine;

namespace BWolf.Utilities.SystemBootstrapping
{
    [DisallowMultipleComponent]
    public class SystemBehaviour : MonoBehaviour
    {
        protected virtual void Reset()
        {
            string type = GetType().Name;
            if (gameObject.name != type)
            {
                Debug.LogWarning($"Name Of {type} and its gameObject name should be the same");
                gameObject.name = type;
            }
        }
    }
}