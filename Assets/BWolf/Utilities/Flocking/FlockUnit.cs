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

        public List<Transform> GetContext(float radius)
        {
            List<Transform> context = new List<Transform>();
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Entity"));
            foreach (Collider c in colliders)
            {
                if (c != unitCollider)
                {
                    context.Add(c.transform);
                }
            }

            return context;
        }
    }
}