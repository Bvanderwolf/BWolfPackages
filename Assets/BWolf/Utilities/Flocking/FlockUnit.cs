using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    /// <summary>class for representing the unit to be flocked</summary>
    public class FlockUnit : MonoBehaviour
    {
        private Collider unitCollider;

        private void Awake()
        {
            unitCollider = GetComponentInChildren<Collider>();
            if (unitCollider == null)
            {
                throw new Exception("No Collider found on this object");
            }
        }

        /// <summary>Moves and rotates this unit according to given velocity</summary>
        public void Flock(Vector3 velocity)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
            transform.position += velocity * Time.deltaTime;
        }

        /// <summary>Returns the context of this unit based on given radius represented by a list of context items</summary>
        public List<ContextItem> GetContext(float radius)
        {
            List<ContextItem> context = new List<ContextItem>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c != unitCollider)
                {
                    context.Add(ContextItem.Create(c.transform, c.gameObject.layer));
                }
            }

            return context;
        }
    }
}