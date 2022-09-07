using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public static class Vector3Extensions
    {
        public static Vector3 Average(this Vector3[] vectors)
        {
            Vector3 average = Vector3.zero;

            for (int i = 0; i < vectors.Length; i++)
                average += vectors[i];

            return average / vectors.Length;
        }
        
        public static float ShortestDistance(this Vector3 position, Vector3[] positions)
        {
            float smallestSqrMagnitude = float.MaxValue;

            for (int i = 0; i < positions.Length; i++)
            {
                float sqrMagnitude = (positions[i] - position).sqrMagnitude;
                if (sqrMagnitude < smallestSqrMagnitude)
                    smallestSqrMagnitude = sqrMagnitude;
            }

            return Mathf.Sqrt(smallestSqrMagnitude);
        }
        
        public static Vector2 Average(this Vector2[] vectors)
        {
            Vector2 average = Vector3.zero;

            for (int i = 0; i < vectors.Length; i++)
                average += vectors[i];

            return average / vectors.Length;
        }
        
        public static float ShortestDistance(this Vector2 position, Vector2[] positions)
        {
            float smallestSqrMagnitude = float.MaxValue;

            for (int i = 0; i < positions.Length; i++)
            {
                float sqrMagnitude = (positions[i] - position).sqrMagnitude;
                if (sqrMagnitude < smallestSqrMagnitude)
                    smallestSqrMagnitude = sqrMagnitude;
            }

            return Mathf.Sqrt(smallestSqrMagnitude);
        }
    }
}
