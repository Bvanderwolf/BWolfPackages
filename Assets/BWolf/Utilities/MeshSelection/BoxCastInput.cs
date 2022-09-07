using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public struct BoxCastInput
    {
        public Vector3 center;

        public Vector3 direction;

        public Vector3 halfExtends;

        public static BoxCastInput FromRays(Ray[] rays, float thickness)
        {
            BoxCastInput input;
            input.center = Vector3.Lerp(rays[0].origin, rays[3].origin, 0.5f);
            input.direction = rays.AverageDirection();

            RaycastHit[] hits = rays.RayCast();
            if (hits.Length == 0)
                throw new InvalidOperationException("Failed to hit anything so can't do box cast.");

            float closestDistance = input.center.Closest(hits.Select(hit => hit.transform.position).ToArray());
            input.halfExtends = new Vector3
            {
                x = (rays[3].origin.x - rays[0].origin.x) * closestDistance,
                y = (rays[3].origin.y - rays[0].origin.y) * closestDistance,
                z = thickness
            };

            return input;
        }
    }
}
