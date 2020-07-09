using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.Flocking
{
    public class FlockUnit : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer body = null;

        private Collider unitCollider;

        private void Awake()
        {
            unitCollider = GetComponentInChildren<Collider>();
            if (unitCollider == null)
            {
                throw new Exception("No Collider found on this object");
            }
        }

        public void Move(Vector3 velocity)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
            transform.position += velocity * Time.deltaTime;
        }

        public void ColorBody(Color color)
        {
            body.material.color = color;
        }

        public List<FlockUnitContext> GetContext(float radius)
        {
            List<FlockUnitContext> context = new List<FlockUnitContext>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider c in colliders)
            {
                if (c != unitCollider)
                {
                    context.Add(FlockUnitContext.Create(c.transform, c.gameObject.layer));
                }
            }

            return context;
        }
    }
}