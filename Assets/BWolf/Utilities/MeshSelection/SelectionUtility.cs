using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BWolf.MeshSelecting
{
    public static class SelectionUtility
    {
        private static readonly Texture2D _texture;

        static SelectionUtility()
        {
            _texture = new Texture2D(1, 1);
            _texture.SetPixel(0, 0, Color.white);
            _texture.Apply();
        }
        
        public static void DrawRect(Rect rect, Color color)
        {
            using (new GUIColor(color)) 
                GUI.DrawTexture(rect, _texture);
        }

        public static void DrawBorder(Rect rect, float thickness, Color color)
        {
            DrawRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            DrawRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            DrawRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            DrawRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect RectFromPositions(Vector2 first, Vector2 second)
        {
            Vector3 topLeft = Vector3.Min(first, second);
            Vector3 bottomRight = Vector3.Max(first, second);
            
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }

        public static Rect ScreenRectFromPositions(Vector2 first, Vector2 second)
        {
            // Move origin from bottom left to top left
            first.y = Screen.height - first.y;
            second.y = Screen.height - second.y;

            return RectFromPositions(first, second);
        }

        public static Vector2[] CornersFromPositions(Vector2 first, Vector2 second)
        {
            // Min and Max to get 2 corners of rectangle regardless of drag direction.
            Vector2 bottomLeft = Vector2.Min(first, second);
            Vector2 topRight = Vector2.Max(first, second);

            // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
            Vector2[] corners =
            {
                new Vector2(bottomLeft.x, topRight.y),
                new Vector2(topRight.x, topRight.y),
                new Vector2(bottomLeft.x, bottomLeft.y),
                new Vector2(topRight.x, bottomLeft.y)
            };
            
            return corners;
        }

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

        public static float Closest(this Vector3 position, Vector3[] positions)
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
