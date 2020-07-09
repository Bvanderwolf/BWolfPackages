using System;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public class FlockUnit : MonoBehaviour
    {
        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponentInChildren<Collider>();
            if (Collider == null)
            {
                throw new Exception("No Collider found on this object");
            }
        }

        public void Move(Vector3 velocity)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
            transform.position += velocity * Time.deltaTime;
        }
    }
}