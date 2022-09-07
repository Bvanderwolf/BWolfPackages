using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public static class RayExtensions
    {
        public static Ray[] RaysFromCorners(Vector2[] corners, Camera camera)
        {
            Ray[] rays = new Ray[4];
            for (int i = 0; i < rays.Length; i++)
                rays[i] = camera.ScreenPointToRay(corners[i]);

            return rays;
        }

        public static Vector3 AverageDirection(this Ray[] rays)
        {
            Vector3 direction = Vector3.zero;

            for (int i = 0; i < rays.Length; i++)
                direction += rays[i].direction;

            return (direction / rays.Length).normalized;
        }

        public static RaycastHit[] RayCast(this Ray[] rays)
        {
            List<RaycastHit> hits = new List<RaycastHit>();
            
            for (int i = 0; i < rays.Length; i++)
                if (Physics.Raycast(rays[i], out RaycastHit hitInfo))
                    hits.Add(hitInfo);
            
            return hits.ToArray();
        }
    }
}