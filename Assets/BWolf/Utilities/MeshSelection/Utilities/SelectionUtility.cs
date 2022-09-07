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
    }
}
